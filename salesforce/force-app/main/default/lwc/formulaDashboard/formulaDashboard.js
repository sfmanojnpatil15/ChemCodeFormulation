/**
 * formulaDashboard.js
 * Main dashboard controller.
 * Migrated from VB.NET Dashboard.vb
 */
import { LightningElement, track, wire } from 'lwc';
import { ShowToastEvent } from 'lightning/platformShowToastEvent';
import { refreshApex } from '@salesforce/apex';

import getFormulas      from '@salesforce/apex/FormulaController.getFormulas';
import getMaterials     from '@salesforce/apex/MaterialController.getMaterials';
import getSalesReps     from '@salesforce/apex/FormulaController.getSalesReps';
import getFormulaSettings from '@salesforce/apex/FormulaController.getFormulaSettings';
import saveFormulaSettings from '@salesforce/apex/FormulaController.saveFormulaSettings';
import deleteFormula from '@salesforce/apex/FormulaController.deleteFormula';
import deactivateMaterial from '@salesforce/apex/MaterialController.deactivateMaterial';

export default class FormulaDashboard extends LightningElement {

    // ─── Tab State ────────────────────────────────────────────────────────────
    @track activeTab = 'formulas';

    // ─── Formula State ────────────────────────────────────────────────────────
    @track formulas = [];
    @track formulasLoaded = false;
    @track filterType = 'Name';
    @track filterValue = '';
    @track timePeriod = '30';
    @track sortedBy = 'Entered_Date__c';
    @track sortedDirection = 'desc';

    @track showFormulaEditor = false;
    @track selectedFormulaId = null;
    @track formulaEditorTitle = 'New Formula';

    // ─── Material State ───────────────────────────────────────────────────────
    @track materials = [];
    @track materialsLoaded = false;
    @track materialSearch = '';
    @track materialCategoryFilter = '';
    @track showMaterialEditor = false;
    @track selectedMaterialId = null;
    @track materialEditorTitle = 'Add Material';

    // ─── Sales Rep State ──────────────────────────────────────────────────────
    @track salesReps = [];
    @track salesRepsLoaded = false;
    @track showSalesRepEditor = false;

    // ─── Settings State ───────────────────────────────────────────────────────
    @track freightPercentage = 0;
    @track newPriceDays = 30;
    @track midPriceDays = 60;
    @track formulaSettingsId = null;

    // ─── Wire ─────────────────────────────────────────────────────────────────
    _formulasWireResult;
    @wire(getFormulas, { filterType: '$filterType', filterValue: '$filterValue', daysBack: '$daysBackNum' })
    wiredFormulas(result) {
        this._formulasWireResult = result;
        if (result.data) {
            this.formulas = this.enrichFormulas(result.data);
            this.formulasLoaded = true;
        } else if (result.error) {
            this.formulasLoaded = true;
            this.showError('Error loading formulas', result.error);
        }
    }

    @wire(getMaterials, { category: '$materialCategoryFilter' })
    wiredMaterials({ data, error }) {
        if (data) {
            this.materials = data;
            this.materialsLoaded = true;
        } else if (error) {
            this.materialsLoaded = true;
        }
    }

    @wire(getSalesReps)
    wiredSalesReps({ data, error }) {
        if (data) {
            this.salesReps = data;
            this.salesRepsLoaded = true;
        }
    }

    @wire(getFormulaSettings)
    wiredSettings({ data, error }) {
        if (data) {
            this.freightPercentage = data.Freight_Percentage__c || 0;
            this.newPriceDays = data.New_Price_Days__c || 30;
            this.midPriceDays = data.Mid_Price_Days__c || 60;
            this.formulaSettingsId = data.Id;
        }
    }

    // ─── Computed ─────────────────────────────────────────────────────────────
    get daysBackNum() {
        return parseInt(this.timePeriod, 10);
    }

    get filteredMaterials() {
        if (!this.materialSearch) return this.materials;
        const search = this.materialSearch.toLowerCase();
        return this.materials.filter(m => m.Name && m.Name.toLowerCase().includes(search));
    }

    get filterOptions() {
        return [
            { label: 'Formula Name', value: 'Name' },
            { label: 'Sales Rep', value: 'SalesRep' },
            { label: 'Customer', value: 'Customer' }
        ];
    }

    get timePeriodOptions() {
        return [
            { label: 'Last 30 Days', value: '30' },
            { label: 'All Time', value: '0' }
        ];
    }

    get categoryOptions() {
        return [
            { label: 'All', value: '' },
            { label: 'Raw Material', value: 'RM' },
            { label: 'Bottle', value: 'Bottle' },
            { label: 'Lid', value: 'Lid' },
            { label: 'Scoop', value: 'Scoop' },
            { label: 'Shipper', value: 'Shipper' },
            { label: 'Stick Pack', value: 'StickPack' },
            { label: 'Sachet', value: 'Sachet' }
        ];
    }

    // ─── Columns ──────────────────────────────────────────────────────────────
    get formulaColumns() {
        return [
            { label: 'Quote #',      fieldName: 'Quote_Number__c',    type: 'text',   sortable: true },
            { label: 'Formula Name', fieldName: 'Name',               type: 'text',   sortable: true },
            { label: 'Type',         fieldName: 'Formula_Type__c',    type: 'text',   sortable: true },
            { label: 'Customer',     fieldName: 'Customer_Name__c',   type: 'text',   sortable: true },
            { label: 'Sales Rep',    fieldName: 'salesRepName',       type: 'text',   sortable: true },
            { label: 'Date',         fieldName: 'Entered_Date__c',    type: 'date',   sortable: true },
            { label: 'Unit Cost',    fieldName: 'Total_Unit_Cost__c', type: 'number', typeAttributes: { minimumFractionDigits: 4 } },
            {
                type: 'action',
                typeAttributes: {
                    rowActions: [
                        { label: 'Open', name: 'open' },
                        { label: 'Delete', name: 'delete' }
                    ]
                }
            }
        ];
    }

    get materialColumns() {
        return [
            { label: 'Material Name',   fieldName: 'Name',             type: 'text', sortable: true },
            { label: 'Category',        fieldName: 'Category__c',      type: 'text', sortable: true },
            { label: 'Supplier',        fieldName: 'Supplier__c',      type: 'text', sortable: true },
            { label: 'Price (per kg)',  fieldName: 'Price__c',         type: 'number', typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Component Code',  fieldName: 'Component_Code__c', type: 'text' },
            { label: 'Vendor Code',     fieldName: 'Vendor_Code__c',   type: 'text' },
            {
                type: 'action',
                typeAttributes: {
                    rowActions: [
                        { label: 'Edit', name: 'edit' },
                        { label: 'Deactivate', name: 'deactivate' }
                    ]
                }
            }
        ];
    }

    get salesRepColumns() {
        return [
            { label: 'Name',     fieldName: 'Name',       type: 'text' },
            { label: 'Rep Code', fieldName: 'Rep_Code__c', type: 'text' },
            { label: 'Email',    fieldName: 'Email__c',   type: 'email' },
            { label: 'Phone',    fieldName: 'Phone__c',   type: 'phone' },
            { label: 'Company',  fieldName: 'Company__c', type: 'text' },
            {
                type: 'action',
                typeAttributes: { rowActions: [{ label: 'Edit', name: 'edit' }] }
            }
        ];
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────
    enrichFormulas(formulas) {
        return formulas.map(f => ({
            ...f,
            salesRepName: f.Sales_Rep__r ? f.Sales_Rep__r.Name : ''
        }));
    }

    // ─── Event Handlers ───────────────────────────────────────────────────────
    handleTabChange(evt) {
        this.activeTab = evt.target.value;
    }

    handleFilterTypeChange(evt) {
        this.filterType = evt.detail.value;
    }

    handleFilterValueChange(evt) {
        this.filterValue = evt.detail.value;
    }

    handleTimePeriodChange(evt) {
        this.timePeriod = evt.detail.value;
    }

    handleMaterialSearch(evt) {
        this.materialSearch = evt.detail.value;
    }

    handleCategoryFilter(evt) {
        this.materialCategoryFilter = evt.detail.value;
    }

    handleSort(evt) {
        this.sortedBy = evt.detail.fieldName;
        this.sortedDirection = evt.detail.sortDirection;
    }

    // Formula actions
    handleNewFormula() {
        this.selectedFormulaId = null;
        this.formulaEditorTitle = 'New Formula';
        this.showFormulaEditor = true;
    }

    handleFormulaRowAction(evt) {
        const action = evt.detail.action.name;
        const row = evt.detail.row;
        if (action === 'open') {
            this.selectedFormulaId = row.Id;
            this.formulaEditorTitle = 'Formula: ' + row.Name + '  (Quote: ' + row.Quote_Number__c + ')';
            this.showFormulaEditor = true;
        } else if (action === 'delete') {
            this.handleDeleteFormula(row.Id, row.Name);
        }
    }

    handleDeleteFormula(formulaId, formulaName) {
        // eslint-disable-next-line no-alert
        if (!confirm('Delete formula "' + formulaName + '"?')) return;
        deleteFormula({ formulaId })
            .then(() => {
                this.showSuccess('Formula deleted');
                refreshApex(this._formulasWireResult);
            })
            .catch(err => this.showError('Delete failed', err));
    }

    handleFormulaSaved() {
        this.showFormulaEditor = false;
        refreshApex(this._formulasWireResult);
        this.showSuccess('Formula saved successfully');
    }

    closeFormulaEditor() {
        this.showFormulaEditor = false;
    }

    // Material actions
    handleAddMaterial() {
        this.selectedMaterialId = null;
        this.materialEditorTitle = 'Add Material';
        this.showMaterialEditor = true;
    }

    handleMaterialRowAction(evt) {
        const action = evt.detail.action.name;
        const row = evt.detail.row;
        if (action === 'edit') {
            this.selectedMaterialId = row.Id;
            this.materialEditorTitle = 'Edit Material: ' + row.Name;
            this.showMaterialEditor = true;
        } else if (action === 'deactivate') {
            deactivateMaterial({ materialId: row.Id })
                .then(() => this.showSuccess('Material deactivated'))
                .catch(err => this.showError('Error', err));
        }
    }

    handleMaterialSaved() {
        this.showMaterialEditor = false;
        this.showSuccess('Material saved');
    }

    closeMaterialEditor() {
        this.showMaterialEditor = false;
    }

    // Sales rep actions
    handleAddSalesRep() {
        this.showSalesRepEditor = true;
    }

    handleSalesRepRowAction(evt) {
        const row = evt.detail.row;
        if (evt.detail.action.name === 'edit') {
            // Open sales rep edit modal (reuse material-manager pattern)
            this.selectedSalesRepId = row.Id;
        }
    }

    // Settings
    handleFreightChange(evt)    { this.freightPercentage = parseFloat(evt.detail.value); }
    handleNewPriceDaysChange(e) { this.newPriceDays = parseInt(e.detail.value, 10); }
    handleMidPriceDaysChange(e) { this.midPriceDays = parseInt(e.detail.value, 10); }

    handleSaveSettings() {
        const settings = {
            Id: this.formulaSettingsId,
            Freight_Percentage__c: this.freightPercentage,
            New_Price_Days__c: this.newPriceDays,
            Mid_Price_Days__c: this.midPriceDays
        };
        saveFormulaSettings({ settings })
            .then(() => this.showSuccess('Settings saved'))
            .catch(err => this.showError('Error saving settings', err));
    }

    // ─── Toast Helpers ────────────────────────────────────────────────────────
    showSuccess(msg) {
        this.dispatchEvent(new ShowToastEvent({ title: 'Success', message: msg, variant: 'success' }));
    }

    showError(title, err) {
        const msg = err?.body?.message || err?.message || 'Unknown error';
        this.dispatchEvent(new ShowToastEvent({ title, message: msg, variant: 'error' }));
    }
}
