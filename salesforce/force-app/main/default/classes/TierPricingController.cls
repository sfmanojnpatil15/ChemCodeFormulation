/**
 * TierPricingController.cls
 * Manages Tier_Pricing__c records for Material record page.
 */
public with sharing class TierPricingController {

    @AuraEnabled(cacheable=true)
    public static List<Tier_Pricing__c> getTierPricingByMaterial(Id materialId) {
        return [
            SELECT Id, Name, QTY__c, Cost__c
            FROM Tier_Pricing__c
            WHERE Material__c = :materialId
            ORDER BY QTY__c ASC
        ];
    }

    @AuraEnabled
    public static Id saveTierPricing(Tier_Pricing__c record) {
        upsert record;
        return record.Id;
    }

    @AuraEnabled
    public static void deleteTierPricing(Id recordId) {
        delete [SELECT Id FROM Tier_Pricing__c WHERE Id = :recordId];
    }
}
