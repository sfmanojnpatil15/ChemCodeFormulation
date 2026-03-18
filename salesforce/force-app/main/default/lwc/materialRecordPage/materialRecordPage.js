/**
 * materialRecordPage.js
 * Material record page with Material, Tier Pricing, and Attachments tabs.
 * Migrated from VB.NET EditMaterial.vb tabbed dialog.
 */
import { LightningElement, api, wire, track } from 'lwc';
import { ShowToastEvent } from 'lightning/platformShowToastEvent';
import { refreshApex } from '@salesforce/apex';
import { deleteRecord } from 'lightning/uiRecordApi';
import getTierPricingByMaterial from '@salesforce/apex/TierPricingController.getTierPricingByMaterial';
import saveTierPricing from '@salesforce/apex/TierPricingController.saveTierPricing';
import deleteTierPricing from '@salesforce/apex/TierPricingController.deleteTierPricing';
import getAttachments from '@salesforce/apex/MaterialController.getAttachments';

export default class MaterialRecordPage extends LightningElement {

    @api recordId;

    @track tierPricingData = [];
    @track tierDraftValues = [];
    @track attachments = [];
    @track selectedContentDocumentId;

    _wiredTierResult;
    _wiredAttachmentsResult;

    tierColumns = [
        { label: 'QTY',  fieldName: 'QTY__c',  type: 'number', editable: true, cellAttributes: { alignment: 'left' } },
        { label: 'Cost', fieldName: 'Cost__c', type: 'number', editable: true, cellAttributes: { alignment: 'left' } },
        { type: 'action', typeAttributes: { rowActions: [{ label: 'Delete', name: 'delete' }] } }
    ];

    attachmentColumns = [
        { label: 'File Name', fieldName: 'Title', type: 'text' },
        {
            label: 'Uploaded Date',
            fieldName: 'CreatedDate',
            type: 'date',
            typeAttributes: {
                day: '2-digit', month: '2-digit', year: 'numeric',
                hour: '2-digit', minute: '2-digit', hour12: false
            }
        }
    ];

    get acceptedFormats() {
        return ['.pdf', '.png', '.jpg', '.jpeg', '.doc', '.docx', '.xls', '.xlsx', '.csv', '.txt'];
    }

    get noFileSelected() {
        return !this.selectedContentDocumentId;
    }

    // ── Wire: Tier Pricing ────────────────────────────────────────────────

    @wire(getTierPricingByMaterial, { materialId: '$recordId' })
    wiredTierPricing(result) {
        this._wiredTierResult = result;
        if (result.data) {
            this.tierPricingData = result.data.map(r => ({ ...r }));
        }
    }

    // ── Wire: Attachments ─────────────────────────────────────────────────

    @wire(getAttachments, { materialId: '$recordId' })
    wiredAttachments(result) {
        this._wiredAttachmentsResult = result;
        if (result.data) {
            this.attachments = result.data;
        }
    }

    // ── Material Tab handlers ─────────────────────────────────────────────

    handleMaterialSuccess() {
        this.showToast('Success', 'Material saved.', 'success');
    }

    handleMaterialError(evt) {
        this.showToast('Error', evt.detail.detail, 'error');
    }

    // ── Tier Pricing Tab handlers ─────────────────────────────────────────

    handleAddTier() {
        const tempId = 'new_' + Date.now();
        this.tierPricingData = [
            ...this.tierPricingData,
            { Id: tempId, QTY__c: 0, Cost__c: 0 }
        ];
    }

    async handleTierSave(evt) {
        const draftValues = evt.detail.draftValues;
        try {
            for (const draft of draftValues) {
                const isNew = String(draft.Id).startsWith('new_');
                const original = this.tierPricingData.find(r => r.Id === draft.Id) || {};
                const record = { ...original, ...draft, Material__c: this.recordId };
                if (isNew) delete record.Id;
                await saveTierPricing({ record });
            }
            this.tierDraftValues = [];
            await refreshApex(this._wiredTierResult);
            this.showToast('Success', 'Tier pricing saved.', 'success');
        } catch (err) {
            this.showToast('Error', this.extractError(err), 'error');
        }
    }

    async handleTierRowAction(evt) {
        const { action, row } = evt.detail;
        if (action.name !== 'delete') return;
        const isNew = String(row.Id).startsWith('new_');
        if (isNew) {
            this.tierPricingData = this.tierPricingData.filter(r => r.Id !== row.Id);
            return;
        }
        try {
            await deleteTierPricing({ recordId: row.Id });
            await refreshApex(this._wiredTierResult);
            this.showToast('Success', 'Tier pricing deleted.', 'success');
        } catch (err) {
            this.showToast('Error', this.extractError(err), 'error');
        }
    }

    // ── Attachments Tab handlers ──────────────────────────────────────────

    handleFileRowSelection(evt) {
        const rows = evt.detail.selectedRows;
        this.selectedContentDocumentId = rows.length > 0 ? rows[0].ContentDocumentId : null;
    }

    handleUploadFinished() {
        refreshApex(this._wiredAttachmentsResult);
    }

    handleDownload() {
        if (this.selectedContentDocumentId) {
            window.open(
                `/sfc/servlet.shepherd/document/download/${this.selectedContentDocumentId}`,
                '_blank'
            );
        }
    }

    async handleDeleteFile() {
        if (!this.selectedContentDocumentId) return;
        try {
            await deleteRecord(this.selectedContentDocumentId);
            this.selectedContentDocumentId = null;
            await refreshApex(this._wiredAttachmentsResult);
            this.showToast('Success', 'File deleted.', 'success');
        } catch (err) {
            this.showToast('Error', this.extractError(err), 'error');
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    showToast(title, message, variant) {
        this.dispatchEvent(new ShowToastEvent({ title, message, variant }));
    }

    extractError(err) {
        return err?.body?.message || err?.message || 'Unknown error';
    }
}
