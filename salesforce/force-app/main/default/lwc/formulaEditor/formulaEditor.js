/**
 * formulaEditor.js
 * Full formula editor: ingredients, manufacturing, packaging, sales pricing.
 * Migrated from VB.NET Formulator2.vb
 */
import { LightningElement, api, track, wire } from 'lwc';
import { ShowToastEvent } from 'lightning/platformShowToastEvent';

import getFormulaDetail         from '@salesforce/apex/FormulaController.getFormulaDetail';
import createFormula            from '@salesforce/apex/FormulaController.createFormula';
import updateFormula            from '@salesforce/apex/FormulaController.updateFormula';
import createNewVersion         from '@salesforce/apex/FormulaController.createNewVersion';
import saveFormulaIngredients   from '@salesforce/apex/FormulaController.saveFormulaIngredients';
import saveBlendingDetail       from '@salesforce/apex/FormulaController.saveBlendingDetail';
import saveTabletingDetail      from '@salesforce/apex/FormulaController.saveTabletingDetail';
import saveEncapsulationDetail  from '@salesforce/apex/FormulaController.saveEncapsulationDetail';
import getBlendingDetail        from '@salesforce/apex/FormulaController.getBlendingDetail';
import getTabletingDetail       from '@salesforce/apex/FormulaController.getTabletingDetail';
import getEncapsulationDetail   from '@salesforce/apex/FormulaController.getEncapsulationDetail';
import getFormulaSettings       from '@salesforce/apex/FormulaController.getFormulaSettings';
import getRawMaterials          from '@salesforce/apex/MaterialController.getRawMaterials';
import searchMaterials          from '@salesforce/apex/MaterialController.searchMaterials';
import calculateFullUnitCost    from '@salesforce/apex/CostCalculationService.calculateFullUnitCost';

let _rowKeyCounter = 0;
function nextKey() { return ++_rowKeyCounter; }

export default class FormulaEditor extends LightningElement {

    @api formulaId;

    // ─── Formula Header ───────────────────────────────────────────────────────
    @track formulaName = '';
    @track formulaType = 'Powder';
    @track customerName = '';
    @track salesRepId = null;
    @track salesRepName = '';
    @track quoteNumber = '';
    @track enteredDate = new Date().toISOString().substring(0, 10);
    @track servingSize = 0;
    @track servingsPerContainer = 30;
    @track currentVersion = 1;
    @track otherIngredients = '';
    @track labTestingPolicy = '';
    @track quoteExpiryDisclaimer = '';
    @track message = '';

    // ─── Ingredients ──────────────────────────────────────────────────────────
    @track ingredients = [];
    @track deletedIngredientIds = [];
    @track freightPercentage = 0;

    // ─── Manufacturing ────────────────────────────────────────────────────────
    @track blending = { Blending_Cost__c: 0, Wastage_Percentage__c: 0, Lab_Cost__c: 0, Flavor_Profile__c: 0 };
    @track tableting = { Type_of_Coating__c: '', Coating_Cost__c: 0, Compression_Cost__c: 0, Wastage_Percentage__c: 0, Lab_Cost__c: 0 };
    @track encapsulation = { Type_of_Capsule__c: '', Capsule_Cost__c: 0, Encapsulation_Cost__c: 0, Wastage_Percentage__c: 0, Lab_Cost__c: 0, Color__c: '' };

    // ─── Packaging ────────────────────────────────────────────────────────────
    @track packagingType = 'Bulk';

    // ─── Costs ────────────────────────────────────────────────────────────────
    @track totalMaterialCost = 0;
    @track manufacturingCost = 0;
    @track totalUnitCost = 0;

    // ─── UI State ─────────────────────────────────────────────────────────────
    @track activeSection = 'ingredients';
    @track showMaterialLookup = false;
    @track materialLookupSearch = '';
    @track materialLookupResults = [];
    @track activeLookupRowKey = null;
    @track isLoading = false;

    // ─── Wire: Formula Settings (freight %) ──────────────────────────────────
    @wire(getFormulaSettings)
    wiredSettings({ data }) {
        if (data) this.freightPercentage = data.Freight_Percentage__c || 0;
    }

    // ─── Lifecycle ────────────────────────────────────────────────────────────
    connectedCallback() {
        if (this.formulaId) {
            this.loadFormulaDetail();
        }
    }

    async loadFormulaDetail() {
        try {
            const wrapper = await getFormulaDetail({ formulaId: this.formulaId });
            const f = wrapper.formula;
            this.formulaName          = f.Name;
            this.formulaType          = f.Formula_Type__c || 'Powder';
            this.customerName         = f.Customer_Name__c || '';
            this.salesRepId           = f.Sales_Rep__c;
            this.salesRepName         = f.Sales_Rep__r ? f.Sales_Rep__r.Name : '';
            this.quoteNumber          = f.Quote_Number__c || '';
            this.enteredDate          = f.Entered_Date__c || new Date().toISOString().substring(0, 10);
            this.servingSize          = f.Serving_Size__c || 0;
            this.servingsPerContainer = f.Servings_Per_Container__c || 30;
            this.currentVersion       = f.Current_Version__c || 1;
            this.otherIngredients     = f.Other_Ingredients__c || '';
            this.labTestingPolicy     = f.Lab_Testing_Cost_Policy__c || '';
            this.quoteExpiryDisclaimer = f.Quote_Expiry_Disclaimer__c || '';
            this.message              = f.Message__c || '';

            this.ingredients = (wrapper.ingredients || []).map(ing => this.enrichIngredient(ing));
            await this.loadManufacturingDetails();
            this.recalculate();
        } catch (err) {
            this.showError('Error loading formula', err);
        }
    }

    async loadManufacturingDetails() {
        try {
            const vn = this.currentVersion;
            const [bl, tb, enc] = await Promise.all([
                getBlendingDetail({ formulaId: this.formulaId, versionNumber: vn }),
                getTabletingDetail({ formulaId: this.formulaId, versionNumber: vn }),
                getEncapsulationDetail({ formulaId: this.formulaId, versionNumber: vn })
            ]);
            if (bl)  this.blending = { ...this.blending, ...bl };
            if (tb)  this.tableting = { ...this.tableting, ...tb };
            if (enc) this.encapsulation = { ...this.encapsulation, ...enc };
        } catch (err) {
            // Non-fatal - manufacturing details may not exist yet
        }
    }

    enrichIngredient(ing) {
        return {
            ...ing,
            rowKey: ing.Id || nextKey(),
            displayIndex: (ing.Ingredient_Index__c || 0) + 1,
            priceDisplay: this.formatNum(ing.Latest_Cost__c || 0),
            priceClass: this.getPriceClass(ing),
            materialCostDisplay: this.formatNum(ing.Material_Cost__c || 0),
            rowClass: 'slds-hint-parent'
        };
    }

    getPriceClass(ing) {
        // Mimic price age coloring: green=new, yellow=mid, red=old
        return '';
    }

    // ─── Computed ─────────────────────────────────────────────────────────────
    get isPowder()  { return this.formulaType === 'Powder'; }
    get isTablet()  { return this.formulaType === 'Tablet'; }
    get isCapsule() { return this.formulaType === 'Capsule'; }

    get totalMaterialCostFormatted()  { return this.formatNum(this.totalMaterialCost); }
    get manufacturingCostFormatted()  { return this.formatNum(this.manufacturingCost); }
    get totalUnitCostFormatted()      { return this.formatNum(this.totalUnitCost); }

    get blendingWastageValue() {
        const mc = this.totalMaterialCost + (this.blending.Blending_Cost__c || 0);
        const wpc = this.blending.Wastage_Percentage__c || 0;
        return this.formatNum(mc * wpc / 100);
    }

    get formulaTypeOptions() {
        return [
            { label: 'Powder', value: 'Powder' },
            { label: 'Tablet', value: 'Tablet' },
            { label: 'Capsule', value: 'Capsule' }
        ];
    }

    get packagingTypeOptions() {
        const opts = [{ label: 'Bulk', value: 'Bulk' }];
        if (this.formulaType === 'Capsule' || this.formulaType === 'Tablet') {
            opts.push({ label: 'Bottles', value: 'Bottles' });
            opts.push({ label: 'Blisters', value: 'Blisters' });
        } else {
            opts.push({ label: 'Bottles', value: 'Bottles' });
            opts.push({ label: 'Stick Packs', value: 'StickPacks' });
            opts.push({ label: 'Sachets', value: 'Sachets' });
            opts.push({ label: 'Stand-Up Bags', value: 'StandUpBags' });
        }
        return opts;
    }

    get materialLookupColumns() {
        return [
            { label: 'Name', fieldName: 'Name', type: 'text' },
            { label: 'Supplier', fieldName: 'Supplier__c', type: 'text' },
            { label: 'Price/kg', fieldName: 'Price__c', type: 'number' },
            { label: 'Component Code', fieldName: 'Component_Code__c', type: 'text' },
            { type: 'action', typeAttributes: { rowActions: [{ label: 'Select', name: 'select' }] } }
        ];
    }

    // ─── Recalculate Costs ────────────────────────────────────────────────────
    async recalculate() {
        try {
            const input = this.buildCostInput();
            const result = await calculateFullUnitCost({ input });
            this.totalMaterialCost = result.totalMaterialCostPerServing;
            this.totalUnitCost     = result.totalUnitCost;

            // Update per-ingredient material costs in the grid
            this.ingredients = this.ingredients.map((ing, idx) => {
                const icr = result.ingredientCosts ? result.ingredientCosts[idx] : null;
                return {
                    ...ing,
                    Mg__c: icr ? this.formatNum(icr.mgWithOverage) : ing.Mg__c,
                    Material_Cost__c: icr ? icr.materialCost : ing.Material_Cost__c,
                    materialCostDisplay: icr ? this.formatNum(icr.materialCost) : ing.materialCostDisplay
                };
            });

            this.calculateManufacturingCost();
        } catch (err) {
            // Silent - calculations are best-effort
        }
    }

    recalculateRow(evt) {
        const rowKey = evt.currentTarget.dataset.id;
        const ing = this.ingredients.find(i => String(i.rowKey) === String(rowKey));
        if (!ing) return;
        this.recalculate();
    }

    calculateManufacturingCost() {
        let mfgCost = 0;
        if (this.isPowder) {
            mfgCost = (this.blending.Blending_Cost__c || 0) +
                      (this.blending.Lab_Cost__c || 0) +
                      (this.blending.Flavor_Profile__c || 0);
        } else if (this.isTablet) {
            mfgCost = (this.tableting.Coating_Cost__c || 0) +
                      (this.tableting.Compression_Cost__c || 0) +
                      (this.tableting.Lab_Cost__c || 0);
        } else if (this.isCapsule) {
            mfgCost = (this.encapsulation.Capsule_Cost__c || 0) +
                      (this.encapsulation.Encapsulation_Cost__c || 0) +
                      (this.encapsulation.Lab_Cost__c || 0);
        }
        this.manufacturingCost = mfgCost;
    }

    buildCostInput() {
        const ingredientInputs = this.ingredients.map(ing => ({
            actualMg:   parseFloat(ing.Actual_Mg__c) || 0,
            potency:    parseFloat(ing.Potency__c) || 100,
            overage:    parseFloat(ing.Overage__c) || 0,
            pricePerKg: parseFloat(ing.Latest_Cost__c) || 0,
            isManual:   ing.Is_Manual__c,
            manualCost: parseFloat(ing.Manual_Cost__c) || 0
        }));

        return {
            formulaId:                this.formulaId,
            versionNumber:            this.currentVersion,
            servingSize:              parseFloat(this.servingSize) || 0,
            servingsPerContainer:     parseInt(this.servingsPerContainer, 10) || 30,
            freightPercentage:        parseFloat(this.freightPercentage) || 0,
            ingredients:              ingredientInputs,
            formulaType:              this.formulaType,
            blendingCost:             parseFloat(this.blending.Blending_Cost__c) || 0,
            blendingWastagePercent:   parseFloat(this.blending.Wastage_Percentage__c) || 0,
            blendingFlavorProfile:    parseFloat(this.blending.Flavor_Profile__c) || 0,
            tabletingCoatingCost:     parseFloat(this.tableting.Coating_Cost__c) || 0,
            tabletingCompressionCost: parseFloat(this.tableting.Compression_Cost__c) || 0,
            tabletingWastagePercent:  parseFloat(this.tableting.Wastage_Percentage__c) || 0,
            encapsulationCapsuleCost: parseFloat(this.encapsulation.Capsule_Cost__c) || 0,
            encapsulationCost:        parseFloat(this.encapsulation.Encapsulation_Cost__c) || 0,
            encapsulationWastagePercent: parseFloat(this.encapsulation.Wastage_Percentage__c) || 0,
            labCost: this.isPowder ? parseFloat(this.blending.Lab_Cost__c) || 0
                   : this.isTablet ? parseFloat(this.tableting.Lab_Cost__c) || 0
                   : parseFloat(this.encapsulation.Lab_Cost__c) || 0
        };
    }

    // ─── Save All ─────────────────────────────────────────────────────────────
    async saveAll() {
        this.isLoading = true;
        try {
            // 1) Save/create formula header
            const formulaRecord = {
                Name:                       this.formulaName,
                Formula_Type__c:            this.formulaType,
                Customer_Name__c:           this.customerName,
                Sales_Rep__c:               this.salesRepId,
                Entered_Date__c:            this.enteredDate,
                Serving_Size__c:            this.servingSize,
                Servings_Per_Container__c:  this.servingsPerContainer,
                Other_Ingredients__c:       this.otherIngredients,
                Lab_Testing_Cost_Policy__c: this.labTestingPolicy,
                Quote_Expiry_Disclaimer__c: this.quoteExpiryDisclaimer,
                Message__c:                 this.message,
                Total_Material_Cost__c:     this.totalMaterialCost,
                Total_Unit_Cost__c:         this.totalUnitCost
            };

            if (!this.formulaId) {
                this.formulaId = await createFormula({ formula: formulaRecord });
            } else {
                await updateFormula({ formula: { Id: this.formulaId, ...formulaRecord } });
            }

            // 2) Save ingredients
            const ingredientsToSave = this.ingredients.map((ing, idx) => ({
                ...ing,
                Id: typeof ing.Id === 'string' && ing.Id.startsWith('0') ? ing.Id : undefined,
                Formula__c: this.formulaId,
                Ingredient_Index__c: idx,
                Version_Number__c: this.currentVersion
            }));
            await saveFormulaIngredients({
                ingredients: ingredientsToSave,
                deletedIngredientIds: this.deletedIngredientIds
            });
            this.deletedIngredientIds = [];

            // 3) Save manufacturing details
            if (this.isPowder) {
                await saveBlendingDetail({ detail: { ...this.blending, Formula__c: this.formulaId, Version_Number__c: this.currentVersion } });
            } else if (this.isTablet) {
                await saveTabletingDetail({ detail: { ...this.tableting, Formula__c: this.formulaId, Version_Number__c: this.currentVersion } });
            } else if (this.isCapsule) {
                await saveEncapsulationDetail({ detail: { ...this.encapsulation, Formula__c: this.formulaId, Version_Number__c: this.currentVersion } });
            }

            this.showSuccess('Formula saved successfully');
            this.dispatchEvent(new CustomEvent('save', { detail: { formulaId: this.formulaId } }));
        } catch (err) {
            this.showError('Error saving formula', err);
        } finally {
            this.isLoading = false;
        }
    }

    async createNewVersion() {
        if (!this.formulaId) {
            this.showToast('info', 'Save the formula first before creating a new version.');
            return;
        }
        try {
            await createNewVersion({ formulaId: this.formulaId, versionNotes: '' });
            this.currentVersion += 1;
            await this.loadManufacturingDetails();
            this.showSuccess('New version created: v' + this.currentVersion);
        } catch (err) {
            this.showError('Error creating version', err);
        }
    }

    // ─── Ingredient Handlers ──────────────────────────────────────────────────
    handleAddIngredient() {
        const newIng = {
            rowKey: nextKey(),
            Formula__c: this.formulaId,
            Ingredient_Index__c: this.ingredients.length,
            displayIndex: this.ingredients.length + 1,
            Material_Name__c: '',
            Component_Code__c: '',
            Vendor_Name__c: '',
            Actual_Mg__c: 0,
            Potency__c: 100,
            Overage__c: 0,
            Mg__c: 0,
            Material_Cost__c: 0,
            Is_Manual__c: false,
            Manual_Cost__c: 0,
            Latest_Cost__c: 0,
            priceDisplay: '0.000000',
            materialCostDisplay: '0.000000',
            priceClass: '',
            rowClass: 'slds-hint-parent'
        };
        this.ingredients = [...this.ingredients, newIng];
    }

    handleIngredientChange(evt) {
        const rowKey = evt.currentTarget.dataset.id;
        const field  = evt.currentTarget.dataset.field;
        const value  = evt.detail ? evt.detail.value : evt.currentTarget.value;
        this.ingredients = this.ingredients.map(ing => {
            if (String(ing.rowKey) === String(rowKey)) {
                return { ...ing, [field]: value };
            }
            return ing;
        });
    }

    handleManualToggle(evt) {
        const rowKey = evt.currentTarget.dataset.id;
        const checked = evt.detail.checked;
        this.ingredients = this.ingredients.map(ing => {
            if (String(ing.rowKey) === String(rowKey)) {
                return { ...ing, Is_Manual__c: checked };
            }
            return ing;
        });
        this.recalculate();
    }

    handleRemoveIngredient(evt) {
        const rowKey = evt.currentTarget.dataset.id;
        const ing = this.ingredients.find(i => String(i.rowKey) === String(rowKey));
        if (ing && ing.Id) {
            this.deletedIngredientIds = [...this.deletedIngredientIds, ing.Id];
        }
        this.ingredients = this.ingredients.filter(i => String(i.rowKey) !== String(rowKey));
        this.recalculate();
    }

    handleIngredientNameClick(evt) {
        this.activeLookupRowKey = evt.currentTarget.dataset.id;
        this.materialLookupSearch = evt.currentTarget.value || '';
        this.showMaterialLookup = true;
        this.loadMaterialLookup(this.materialLookupSearch);
    }

    async loadMaterialLookup(searchTerm) {
        try {
            this.materialLookupResults = await searchMaterials({ searchTerm: searchTerm || '', category: 'RM' });
        } catch (err) {
            this.materialLookupResults = [];
        }
    }

    handleMaterialLookupSearch(evt) {
        this.materialLookupSearch = evt.detail.value;
        this.loadMaterialLookup(this.materialLookupSearch);
    }

    handleMaterialSelect(evt) {
        if (evt.detail.action.name !== 'select') return;
        const mat = evt.detail.row;
        const rowKey = this.activeLookupRowKey;
        this.ingredients = this.ingredients.map(ing => {
            if (String(ing.rowKey) === String(rowKey)) {
                return {
                    ...ing,
                    Material__c: mat.Id,
                    Material_Name__c: mat.Name,
                    Component_Code__c: mat.Component_Code__c || '',
                    Vendor_Name__c: mat.Supplier__c || '',
                    Latest_Cost__c: mat.Price__c || 0,
                    priceDisplay: this.formatNum(mat.Price__c || 0)
                };
            }
            return ing;
        });
        this.showMaterialLookup = false;
        this.activeLookupRowKey = null;
        this.recalculate();
    }

    closeMaterialLookup() {
        this.showMaterialLookup = false;
    }

    // ─── Formula Header Handlers ──────────────────────────────────────────────
    handleNameChange(evt)         { this.formulaName = evt.detail.value; }
    handleFormulaTypeChange(evt)  { this.formulaType = evt.detail.value; this.recalculate(); }
    handleCustomerChange(evt)     { this.customerName = evt.detail.value; }
    handleServingSizeChange(evt)  { this.servingSize = parseFloat(evt.detail.value) || 0; }
    handleServingsChange(evt)     { this.servingsPerContainer = parseInt(evt.detail.value, 10) || 30; }
    handleDateChange(evt)         { this.enteredDate = evt.detail.value; }
    handlePackagingTypeChange(evt){ this.packagingType = evt.detail.value; }
    handleSectionChange(evt)      { this.activeSection = evt.target.value; }

    openSalesRepLookup() {
        // Could open a lookup modal for sales reps
    }

    // ─── Manufacturing Handlers ───────────────────────────────────────────────
    handleBlendingChange(evt) {
        const field = evt.currentTarget.dataset.field;
        this.blending = { ...this.blending, [field]: parseFloat(evt.detail.value) || 0 };
        this.recalculate();
    }

    handleTabletingChange(evt) {
        const field = evt.currentTarget.dataset.field;
        const val = evt.detail.value;
        this.tableting = { ...this.tableting, [field]: isNaN(parseFloat(val)) ? val : parseFloat(val) };
        this.recalculate();
    }

    handleEncapsulationChange(evt) {
        const field = evt.currentTarget.dataset.field;
        const val = evt.detail.value;
        this.encapsulation = { ...this.encapsulation, [field]: isNaN(parseFloat(val)) ? val : parseFloat(val) };
        this.recalculate();
    }

    handleTermsChange(evt) {
        const field = evt.currentTarget.dataset.field;
        const val = evt.detail.value;
        if (field === 'otherIngredients')       this.otherIngredients = val;
        else if (field === 'labTestingPolicy')  this.labTestingPolicy = val;
        else if (field === 'quoteExpiryDisclaimer') this.quoteExpiryDisclaimer = val;
        else if (field === 'message')           this.message = val;
    }

    handlePackagingChange(evt)   { /* Packaging editor notifies cost update */ }
    handleSalesPriceChange(evt)  { /* Sales pricing grid updates */ }

    // ─── Utilities ────────────────────────────────────────────────────────────
    formatNum(val) {
        if (val == null) return '0.000000';
        return parseFloat(val).toFixed(6);
    }

    showSuccess(msg) {
        this.dispatchEvent(new ShowToastEvent({ title: 'Success', message: msg, variant: 'success' }));
    }

    showError(title, err) {
        const msg = err?.body?.message || err?.message || JSON.stringify(err);
        this.dispatchEvent(new ShowToastEvent({ title, message: msg, variant: 'error' }));
    }

    showToast(variant, msg) {
        this.dispatchEvent(new ShowToastEvent({ title: variant, message: msg, variant }));
    }
}
