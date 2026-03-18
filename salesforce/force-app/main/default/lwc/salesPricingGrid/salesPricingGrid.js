/**
 * salesPricingGrid.js
 * Sales pricing table for BULK / BOX / BULKBAGS.
 * Migrated from VB.NET Formulator2.vb sales grid handlers.
 */
import { LightningElement, api, track, wire } from 'lwc';
import { ShowToastEvent } from 'lightning/platformShowToastEvent';
import getSalesDetails  from '@salesforce/apex/FormulaController.getSalesDetails';
import saveSalesDetails from '@salesforce/apex/FormulaController.saveSalesDetails';

let _rowCounter = 0;
function nextKey() { return 'row_' + (++_rowCounter); }

export default class SalesPricingGrid extends LightningElement {
    @api formulaId;
    @api versionNumber = 1;
    @api unitCost = 0;
    @api packagingType = 'Bulk';

    @track allRows = [];
    @track deletedIds = [];
    @track activeSaleType = 'BULK';
    @track draftValues = [];

    connectedCallback() {
        if (this.formulaId) this.loadSalesDetails();
    }

    async loadSalesDetails() {
        try {
            const rows = await getSalesDetails({
                formulaId: this.formulaId,
                saleType: null,
                versionNumber: this.versionNumber
            });
            this.allRows = (rows || []).map(r => ({ ...r, rowKey: r.Id || nextKey() }));
        } catch (err) { /* silent */ }
    }

    get bulkRows() { return this.allRows.filter(r => r.Sale_Type__c === 'BULK'); }
    get boxRows()  { return this.allRows.filter(r => r.Sale_Type__c === 'BOX'); }
    get bagRows()  { return this.allRows.filter(r => r.Sale_Type__c === 'BULKBAGS'); }

    get saleColumns() {
        return [
            { label: 'Unit Size', fieldName: 'Unit_Size__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 2 } },
            { label: 'Quantity', fieldName: 'Quantity__c', type: 'number', editable: true },
            { label: 'Margin %', fieldName: 'Margin_Percentage__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 2 } },
            { label: 'Sales Price', fieldName: 'Sales_Price__c', type: 'number', typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Override Price', fieldName: 'Overridden_Sales_Price__c', type: 'number', editable: true, typeAttributes: { minimumFractionDigits: 4 } },
            { label: 'Overridden', fieldName: 'Is_Overridden__c', type: 'boolean', editable: true },
            { type: 'action', typeAttributes: { rowActions: [{ label: 'Delete', name: 'delete' }] } }
        ];
    }

    handleSaleTypeChange(evt) { this.activeSaleType = evt.target.value; }

    handleAddRow(evt) {
        const saleType = evt.currentTarget.dataset.type || this.activeSaleType;
        const newRow = {
            rowKey: nextKey(),
            Formula__c: this.formulaId,
            Sale_Type__c: saleType,
            Unit_Size__c: 0,
            Quantity__c: 0,
            Margin_Percentage__c: 30,
            Sales_Price__c: this.calcSalesPrice(this.unitCost, 30),
            Overridden_Sales_Price__c: 0,
            Is_Overridden__c: false,
            Version_Number__c: this.versionNumber
        };
        this.allRows = [...this.allRows, newRow];
    }

    handleCellChange(evt) {
        const draftVals = evt.detail.draftValues;
        draftVals.forEach(draft => {
            this.allRows = this.allRows.map(row => {
                if (row.rowKey === draft.rowKey) {
                    const updated = { ...row, ...draft };
                    // Recalculate sales price when margin changes
                    if (draft.Margin_Percentage__c !== undefined) {
                        updated.Sales_Price__c = this.calcSalesPrice(this.unitCost, parseFloat(draft.Margin_Percentage__c));
                    }
                    return updated;
                }
                return row;
            });
        });
        this.draftValues = [];
    }

    handleRowAction(evt) {
        if (evt.detail.action.name === 'delete') {
            const row = evt.detail.row;
            if (row.Id) this.deletedIds.push(row.Id);
            this.allRows = this.allRows.filter(r => r.rowKey !== row.rowKey);
        }
    }

    async handleSave() {
        try {
            const toSave = this.allRows.map(r => ({
                Id: (r.Id && r.Id.startsWith('0')) ? r.Id : undefined,
                Formula__c: this.formulaId,
                Sale_Type__c: r.Sale_Type__c,
                Unit_Size__c: r.Unit_Size__c,
                Quantity__c: r.Quantity__c,
                Margin_Percentage__c: r.Margin_Percentage__c,
                Sales_Price__c: r.Sales_Price__c,
                Overridden_Sales_Price__c: r.Overridden_Sales_Price__c,
                Is_Overridden__c: r.Is_Overridden__c,
                Version_Number__c: this.versionNumber
            }));
            await saveSalesDetails({ records: toSave, deletedIds: this.deletedIds });
            this.deletedIds = [];
            this.dispatchEvent(new ShowToastEvent({ title: 'Success', message: 'Sales pricing saved.', variant: 'success' }));
        } catch (err) {
            const msg = err?.body?.message || 'Unknown error';
            this.dispatchEvent(new ShowToastEvent({ title: 'Error', message: msg, variant: 'error' }));
        }
    }

    calcSalesPrice(unitCost, marginPct) {
        if (!unitCost || marginPct >= 100) return 0;
        return parseFloat((unitCost / (1 - marginPct / 100)).toFixed(6));
    }
}
