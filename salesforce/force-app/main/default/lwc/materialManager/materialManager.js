/**
 * materialManager.js
 * Create/Edit material form.
 * Migrated from VB.NET NewMaterial.vb and EditMaterial.vb
 */
import { LightningElement, api, track, wire } from 'lwc';
import { ShowToastEvent } from 'lightning/platformShowToastEvent';
import getMaterialById from '@salesforce/apex/MaterialController.getMaterialById';
import saveMaterial    from '@salesforce/apex/MaterialController.saveMaterial';

export default class MaterialManager extends LightningElement {

    @api materialId;

    @track materialName = '';
    @track category = 'RM';
    @track supplier = '';
    @track price = 0;
    @track componentCode = '';
    @track vendorCode = '';
    @track casePack = 0;
    @track isActive = true;
    @track isSaving = false;

    connectedCallback() {
        if (this.materialId) {
            this.loadMaterial();
        }
    }

    async loadMaterial() {
        try {
            const mat = await getMaterialById({ materialId: this.materialId });
            this.materialName  = mat.Name;
            this.category      = mat.Category__c;
            this.supplier      = mat.Supplier__c || '';
            this.price         = mat.Price__c || 0;
            this.componentCode = mat.Component_Code__c || '';
            this.vendorCode    = mat.Vendor_Code__c || '';
            this.casePack      = mat.Case_Pack__c || 0;
            this.isActive      = mat.Is_Active__c !== false;
        } catch (err) {
            this.showError('Error loading material', err);
        }
    }

    get categoryOptions() {
        return [
            { label: 'Raw Material (RM)', value: 'RM' },
            { label: 'Bottle', value: 'Bottle' },
            { label: 'Lid', value: 'Lid' },
            { label: 'Scoop', value: 'Scoop' },
            { label: 'Shipper', value: 'Shipper' },
            { label: 'Stick Pack', value: 'StickPack' },
            { label: 'Sachet', value: 'Sachet' },
            { label: 'Other', value: 'Other' }
        ];
    }

    handleNameChange(evt)          { this.materialName  = evt.detail.value; }
    handleCategoryChange(evt)      { this.category      = evt.detail.value; }
    handleSupplierChange(evt)      { this.supplier      = evt.detail.value; }
    handlePriceChange(evt)         { this.price         = parseFloat(evt.detail.value) || 0; }
    handleComponentCodeChange(evt) { this.componentCode = evt.detail.value; }
    handleVendorCodeChange(evt)    { this.vendorCode    = evt.detail.value; }
    handleCasePackChange(evt)      { this.casePack      = parseFloat(evt.detail.value) || 0; }
    handleIsActiveChange(evt)      { this.isActive      = evt.detail.checked; }

    async handleSave() {
        if (!this.materialName) {
            this.dispatchEvent(new ShowToastEvent({ title: 'Validation', message: 'Material name is required.', variant: 'warning' }));
            return;
        }
        this.isSaving = true;
        try {
            const material = {
                Id: this.materialId || undefined,
                Name:              this.materialName,
                Category__c:       this.category,
                Supplier__c:       this.supplier,
                Price__c:          this.price,
                Component_Code__c: this.componentCode,
                Vendor_Code__c:    this.vendorCode,
                Case_Pack__c:      this.casePack,
                Is_Active__c:      this.isActive
            };
            const savedId = await saveMaterial({ material });
            this.materialId = savedId;
            this.dispatchEvent(new ShowToastEvent({ title: 'Success', message: 'Material saved.', variant: 'success' }));
            this.dispatchEvent(new CustomEvent('save', { detail: { materialId: savedId } }));
        } catch (err) {
            this.showError('Error saving material', err);
        } finally {
            this.isSaving = false;
        }
    }

    handleCancel() {
        this.dispatchEvent(new CustomEvent('close'));
    }

    showError(title, err) {
        const msg = err?.body?.message || err?.message || 'Unknown error';
        this.dispatchEvent(new ShowToastEvent({ title, message: msg, variant: 'error' }));
    }
}
