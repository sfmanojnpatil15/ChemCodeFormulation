/**
 * packagingEditor.js
 * Packaging cost entry: Bottles, Stick Packs, Sachets, Stand-Up Bags, Blisters.
 * Migrated from VB.NET Formulator2.vb packaging panels.
 */
import { LightningElement, api, track, wire } from 'lwc';
import { ShowToastEvent } from 'lightning/platformShowToastEvent';
import getBottlePackaging   from '@salesforce/apex/FormulaController.getBottlePackaging';
import saveBottlePackaging  from '@salesforce/apex/FormulaController.saveBottlePackaging';
import getStickPackDetails  from '@salesforce/apex/FormulaController.getStickPackDetails';
import saveStickPackDetails from '@salesforce/apex/FormulaController.saveStickPackDetails';
import getBlisterDetails    from '@salesforce/apex/FormulaController.getBlisterDetails';
import saveBlisterDetails   from '@salesforce/apex/FormulaController.saveBlisterDetails';
import getStandUpBagDetails from '@salesforce/apex/FormulaController.getStandUpBagDetails';
import saveStandUpBagDetails from '@salesforce/apex/FormulaController.saveStandUpBagDetails';

let _key = 0;
const nextKey = () => 'pk_' + (++_key);

export default class PackagingEditor extends LightningElement {
    @api formulaId;
    @api versionNumber = 1;
    @api packagingType = 'Bulk';
    @api formulaType = 'Powder';
    @api unitCost = 0;

    @track bottleRows = [];
    @track deletedBottleIds = [];
    @track stickPackRows = [];
    @track stickPackDraftValues = [];
    @track deletedStickPackIds = [];
    @track blister = {};
    @track bagRows2 = [];
    @track bagDraftValues = [];
    @track deletedBagIds = [];

    get isBottles()    { return this.packagingType === 'Bottles'; }
    get isStickPacks() { return this.packagingType === 'StickPacks'; }
    get isBlisters()   { return this.packagingType === 'Blisters'; }
    get isStandUpBags(){ return this.packagingType === 'StandUpBags'; }
    get isSachets()    { return this.packagingType === 'Sachets'; }

    get blisterTotalCostFormatted() {
        const b = this.blister;
        const total = (b.Blister_Cost__c || 0) + (b.Pack_Out__c || 0) + (b.Shrink_Wrap__c || 0)
                    + (b.Wafer_Seal__c || 0) + (b.Printing_Cost__c || 0) + (b.Printing_Plates__c || 0)
                    + (b.Art_Preparation__c || 0) + (b.Tooling_Cost__c || 0) + (b.Other_Cost__c || 0)
                    + (b.Shipper_Case_Cost__c || 0) + (b.Research_Development_Bulk__c || 0);
        return parseFloat(total).toFixed(6);
    }

    get stickPackColumns() {
        return [
            { label: 'Size', fieldName: 'Size_Count__c', type: 'text', editable: true },
            { label: 'Material', fieldName: 'Material__c', type: 'text', editable: true },
            { label: 'Qty', fieldName: 'Quantity__c', type: 'text', editable: true },
            { label: 'Filling Cost', fieldName: 'Filling_Cost__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Printing Cost', fieldName: 'Printing_Cost__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Shrink Wrap', fieldName: 'Shrink_Wrap__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Wafer Seal', fieldName: 'Wafer_Seal__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Art Prep', fieldName: 'Art_Preparation__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'R&D (Bulk)', fieldName: 'Research_Development_Bulk__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'R&D (Box)', fieldName: 'Research_Development_Box__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { type: 'action', typeAttributes: { rowActions: [{ label: 'Delete', name: 'delete' }] } }
        ];
    }

    get bagColumns() {
        return [
            { label: 'Bag Size', fieldName: 'Bag_Size__c', type: 'text', editable: true },
            { label: 'Material', fieldName: 'Bag_Material__c', type: 'text', editable: true },
            { label: 'Fill Weight', fieldName: 'Fill_Weight__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Bag Cost', fieldName: 'Bag_Cost__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Printing', fieldName: 'Printing_Cost__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Shipper Qty', fieldName: 'Shipper_Case_Count__c', type: 'number', editable: true },
            { label: 'Shipper Cost', fieldName: 'Shipper_Case_Cost__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'R&D', fieldName: 'Research_Development__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { type: 'action', typeAttributes: { rowActions: [{ label: 'Delete', name: 'delete' }] } }
        ];
    }

    connectedCallback() {
        if (this.formulaId) this.loadData();
    }

    async loadData() {
        try {
            const vn = this.versionNumber;
            if (this.isBottles) {
                const rows = await getBottlePackaging({ formulaId: this.formulaId, versionNumber: vn });
                this.bottleRows = (rows || []).map(r => ({ ...r, rowKey: r.Id || nextKey() }));
            } else if (this.isStickPacks) {
                const rows = await getStickPackDetails({ formulaId: this.formulaId, versionNumber: vn });
                this.stickPackRows = (rows || []).map(r => ({ ...r, rowKey: r.Id || nextKey() }));
            } else if (this.isBlisters) {
                const rows = await getBlisterDetails({ formulaId: this.formulaId, versionNumber: vn });
                this.blister = rows && rows.length > 0 ? rows[0] : {};
            } else if (this.isStandUpBags) {
                const rows = await getStandUpBagDetails({ formulaId: this.formulaId, versionNumber: vn });
                this.bagRows2 = (rows || []).map(r => ({ ...r, rowKey: r.Id || nextKey() }));
            }
        } catch (err) { /* Non-fatal */ }
    }

    // Bottle handlers
    handleAddBottle() {
        this.bottleRows = [...this.bottleRows, {
            rowKey: nextKey(), Formula__c: this.formulaId, Version_Number__c: this.versionNumber,
            Binding_Index__c: this.bottleRows.length, Size_Count__c: 0, Category__c: '',
            Material_Name__c: '', Vendor_Name__c: '', Unit_Cost__c: 0, Component_Code__c: '', Is_Default__c: false
        }];
    }

    handleBottleChange(evt) {
        const rowKey = evt.currentTarget.dataset.id;
        const field = evt.currentTarget.dataset.field;
        const value = evt.target.value;
        this.bottleRows = this.bottleRows.map(r =>
            r.rowKey === rowKey ? { ...r, [field]: value } : r
        );
    }

    handleDefaultToggle(evt) {
        const rowKey = evt.currentTarget.dataset.id;
        const checked = evt.detail.checked;
        this.bottleRows = this.bottleRows.map(r => ({ ...r, Is_Default__c: r.rowKey === rowKey ? checked : false }));
    }

    handleDeleteBottle(evt) {
        const rowKey = evt.currentTarget.dataset.id;
        const row = this.bottleRows.find(r => r.rowKey === rowKey);
        if (row?.Id) this.deletedBottleIds.push(row.Id);
        this.bottleRows = this.bottleRows.filter(r => r.rowKey !== rowKey);
    }

    async handleSaveBottles() {
        try {
            await saveBottlePackaging({ records: this.bottleRows, deletedIds: this.deletedBottleIds });
            this.deletedBottleIds = [];
            this.dispatchEvent(new ShowToastEvent({ title: 'Success', message: 'Bottle packaging saved.', variant: 'success' }));
        } catch (err) {
            this.dispatchEvent(new ShowToastEvent({ title: 'Error', message: err?.body?.message || 'Error', variant: 'error' }));
        }
    }

    // Stick pack handlers
    handleAddStickPack() {
        this.stickPackRows = [...this.stickPackRows, {
            rowKey: nextKey(), Formula__c: this.formulaId, Version_Number__c: this.versionNumber
        }];
    }

    handleStickPackChange(evt) {
        const drafts = evt.detail.draftValues;
        drafts.forEach(d => {
            this.stickPackRows = this.stickPackRows.map(r => r.rowKey === d.rowKey ? { ...r, ...d } : r);
        });
        this.stickPackDraftValues = [];
    }

    async handleSaveStickPacks() {
        try {
            await saveStickPackDetails({ records: this.stickPackRows, deletedIds: this.deletedStickPackIds });
            this.deletedStickPackIds = [];
            this.dispatchEvent(new ShowToastEvent({ title: 'Success', message: 'Stick packs saved.', variant: 'success' }));
        } catch (err) {
            this.dispatchEvent(new ShowToastEvent({ title: 'Error', message: err?.body?.message || 'Error', variant: 'error' }));
        }
    }

    // Blister handlers
    handleBlisterChange(evt) {
        const field = evt.currentTarget.dataset.field;
        const val = parseFloat(evt.detail.value) || 0;
        this.blister = { ...this.blister, [field]: val };
    }

    async handleSaveBlisters() {
        try {
            const record = { ...this.blister, Formula__c: this.formulaId, Version_Number__c: this.versionNumber };
            await saveBlisterDetails({ records: [record], deletedIds: [] });
            this.dispatchEvent(new ShowToastEvent({ title: 'Success', message: 'Blister details saved.', variant: 'success' }));
        } catch (err) {
            this.dispatchEvent(new ShowToastEvent({ title: 'Error', message: err?.body?.message || 'Error', variant: 'error' }));
        }
    }

    // Stand-up bag handlers
    handleAddBag() {
        this.bagRows2 = [...this.bagRows2, { rowKey: nextKey(), Formula__c: this.formulaId, Version_Number__c: this.versionNumber }];
    }

    handleBagChange(evt) {
        const drafts = evt.detail.draftValues;
        drafts.forEach(d => {
            this.bagRows2 = this.bagRows2.map(r => r.rowKey === d.rowKey ? { ...r, ...d } : r);
        });
        this.bagDraftValues = [];
    }

    async handleSaveBags() {
        try {
            await saveStandUpBagDetails({ records: this.bagRows2, deletedIds: this.deletedBagIds });
            this.deletedBagIds = [];
            this.dispatchEvent(new ShowToastEvent({ title: 'Success', message: 'Bags saved.', variant: 'success' }));
        } catch (err) {
            this.dispatchEvent(new ShowToastEvent({ title: 'Error', message: err?.body?.message || 'Error', variant: 'error' }));
        }
    }

    recalculate() {
        this.dispatchEvent(new CustomEvent('packagingchange'));
    }
}
