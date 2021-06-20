﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace TallyConnector.Models
{
    [Serializable]
    [XmlRoot(ElementName = "VOUCHER",Namespace ="")]
    public class Voucher : TallyXmlJson
    {
        public Voucher()
        {
            _DeliveryNotes = new();
            Ledgers = new();
        }

        [XmlElement(ElementName = "MASTERID")]
        public int? TallyId { get; set; }

        [XmlElement(ElementName = "DATE")]
        public string VchDate { get; set; }


        [XmlElement(ElementName = "VOUCHERTYPENAME")]
        public string VoucherType { get; set; }


        [XmlElement(ElementName = "VOUCHERNUMBER")]
        public string VoucherNumber { get; set; }

        [XmlElement(ElementName = "ISOPTIONAL")]
        public string Isoptional { get; set; }

        [XmlElement(ElementName = "EFFECTIVEDATE")]
        public string EffectiveDate { get; set; }

        [XmlElement(ElementName = "NARRATION")]
        public string Narration { get; set; }

        //Dispatch Details
        [XmlIgnore]
        public string DeliveryNoteNo { get; set; }

        [XmlIgnore]
        public string ShippingDate { get; set; }

        private DeliveryNotes _DeliveryNotes;

        

        [JsonIgnore]
        [XmlElement(ElementName = "BASICSHIPDELIVERYNOTE")]
        public DeliveryNotes DeliveryNotes
        {
            get 
            {
                DeliveryNoteNo = _DeliveryNotes.DeliveryNote;
                ShippingDate = _DeliveryNotes.ShippingDate;
                return _DeliveryNotes; 
            }
            set 
            {
                _DeliveryNotes.ShippingDate = value.ShippingDate;
                _DeliveryNotes.DeliveryNote = value.DeliveryNote;
                _DeliveryNotes = value; 
            }
        }

        [XmlElement(ElementName = "BASICSHIPDOCUMENTNO")]
        public string DispatchDocNo { get; set; }
        
        [XmlElement(ElementName = "BASICSHIPPEDBY")]
        public string BasicShippedBy { get; set; }

        [XmlElement(ElementName = "BASICFINALDESTINATION")]
        public string Destination { get; set; }
        
        [XmlElement(ElementName = "EICHECKPOST")]
        public string CarrierName { get; set; }

        [XmlElement(ElementName = "BILLOFLADINGNO")]
        public string BillofLandingNo { get; set; }
        
        [XmlElement(ElementName = "BILLOFLADINGDATE")]
        public string BillofLandingDate { get; set; }

        [XmlElement(ElementName = "BASICSHIPVESSELNO")]
        public string VehicleNo { get; set; }



        //Party Details
        [XmlElement(ElementName = "PARTYNAME")]
        public string PartyName { get; set; }

        [XmlElement(ElementName = "STATENAME")]
        public string State { get; set; }

        [XmlElement(ElementName = "COUNTRYOFRESIDENCE")]
        public string Country { get; set; }

        [XmlElement(ElementName = "GSTREGISTRATIONTYPE")]
        public string RegistrationType { get; set; }

        [XmlElement(ElementName = "PARTYGSTIN")]
        public string PartyGSTIN { get; set; }

        [XmlElement(ElementName = "PLACEOFSUPPLY")]
        public string PlaceOfSupply { get; set; }

        //Consignee Details

        [XmlElement(ElementName = "BASICBUYERNAME")]
        public string ConsigneeName { get; set; }

        [XmlElement(ElementName = "CONSIGNEEMAILINGNAME")]
        public string ConsigneeMailingName { get; set; }

        [XmlElement(ElementName = "CONSIGNEESTATENAME")]
        public string ConsigneeState { get; set; }

        [XmlElement(ElementName = "CONSIGNEECOUNTRYNAME")]
        public string ConsigneeCountry { get; set; }

        [XmlElement(ElementName = "CONSIGNEEGSTIN")]
        public string ConsigneeGSTIN { get; set; }




        [XmlElement(ElementName = "ISCANCELLED")]
        public string IsCancelled { get; set; }

        [XmlElement(ElementName = "ALLLEDGERENTRIES.LIST")]
        public List<IVoucherLedger> Ledgers { get; set; }

        [JsonIgnore]
        [XmlAttribute(AttributeName = "DATE")]
        public string Dt
        {
            get
            {
                return VchDate;
            }
            set
            {
                VchDate = value;
            }
        }

        [JsonIgnore]
        [XmlAttribute(AttributeName = "TAGNAME")]
        public string TagName
        {
            get
            {

                return "Voucher Number";
            }
            set
            {
            }
        }

        [JsonIgnore]
        [XmlAttribute(AttributeName = "TAGVALUE")]
        public string TagValue
        {
            get
            {
                return VoucherNumber;
            }
            set
            {
                VoucherNumber = value;
            }
        }


        /// <summary>
        /// Accepted Values //Create, Alter, Delete
        /// </summary>
        [JsonIgnore]
        [XmlAttribute(AttributeName = "Action")]
        public string Action { get; set; }

        [JsonIgnore]
        [XmlAttribute(AttributeName = "VCHTYPE")]
        public string VchType
        {
            get
            {
                return VoucherType;
            }
            set
            {
                VoucherType = value;
            }
        }

        

        public void OrderLedgers()
        {
            if (VchType != "Contra" && VchType != "Purchase" && VchType != "Receipt" && VchType != "Credit Note")
            {
                Ledgers.Sort((x, y) => y.LedgerName.CompareTo(x.LedgerName));//First Sort Ledger list Using Ledger Names
                Ledgers.Sort((x, y) => y.Amount.CompareTo(x.Amount)); //Next sort Ledger List Using Ledger Amounts

            }
            else
            {
                Ledgers.Sort((x, y) => x.LedgerName.CompareTo(y.LedgerName));//First Sort Ledger list Using Ledger Names
                Ledgers.Sort((x, y) => x.Amount.CompareTo(y.Amount)); //Next sort Ledger List Using Ledger Amounts
            }

            //Looop Through all Ledgers
            Ledgers.ForEach(c =>
            {
                //Sort Bill Allocations
                c.BillAllocations.Sort((x, y) => x.Name.CompareTo(y.Name)); //First Sort BillAllocations Using Bill Numbers
                c.BillAllocations.Sort((x, y) => x.Amount.CompareTo(y.Amount));//Next sort BillAllocationst Using  Amounts

                c.CostCategoryAllocations.Sort((x, y) => x.CostCategoryName.CompareTo(y.CostCategoryName));

                c.CostCategoryAllocations.ForEach(cc =>
                {
                    cc.CostCenterAllocations.Sort((x, y) => x.Name.CompareTo(y.Name));
                    cc.CostCenterAllocations.Sort((x, y) => x.Amount.CompareTo(y.Amount));
                });
                //sort Inventory Allocations
                c.InventoryAllocations.Sort((x, y) => x.ActualQuantity.CompareTo(y.ActualQuantity));
                c.InventoryAllocations.Sort((x, y) => x.Amount.CompareTo(y.Amount));

                c.InventoryAllocations.ForEach(inv =>
                {
                    inv.BatchAllocations.Sort((x, y) => x.GodownName.CompareTo(y.GodownName));
                    inv.BatchAllocations.Sort((x, y) => x.Amount.CompareTo(y.Amount));

                    inv.CostCategoryAllocations.Sort((x, y) => x.CostCategoryName.CompareTo(y.CostCategoryName));

                    inv.CostCategoryAllocations.ForEach(cc =>
                    {
                        cc.CostCenterAllocations.Sort((x, y) => x.Name.CompareTo(y.Name));
                        cc.CostCenterAllocations.Sort((x, y) => x.Amount.CompareTo(y.Amount));
                    });
                });

            });
        }

        public new string GetJson()
        {
            OrderLedgers();

            return base.GetJson();
        }

        public new string GetXML()
        {
            OrderLedgers();
            BillAllocComputeJD();
            return base.GetXML();
        }
        public void BillAllocComputeJD()
        {
            //DateTime Vchdate = DateTime.TryParseExact(VchDate, "yyyyddMM",);
            //double JDval = Vchdate.ToOADate();
            //this.Ledgers.ForEach(Ledg => Ledg.BillAllocations.ForEach(BillAlloc => BillAlloc.BillCP.JD = $"{JDval}"));
        }
    }

    [XmlRoot(ElementName = "LEDGERENTRIES.LIST")]
    public class EVoucherLedger : IVoucherLedger
    {

    }

    [XmlRoot(ElementName = "ALLLEDGERENTRIES.LIST")]
    public class IVoucherLedger
    {

        public IVoucherLedger()
        {
            BillAllocations = new();
            InventoryAllocations = new();
            CostCategoryAllocations = new();
        }


        [XmlElement(ElementName = "LEDGERNAME")]
        public string LedgerName { get; set; }


        [XmlElement(ElementName = "ISDEEMEDPOSITIVE")]
        public string IsDeemedPositive {
            get 
            {
                if (_Amount != null)
                {
                    if (int.Parse(_Amount) > 0)
                    {
                        return "No";
                    }
                    else
                    {
                        return "Yes";
                    }
                }
                return null;
                
            }
            set { }
        }

        private string _Amount;

        public string ForexAmount { get; set; }

        public string RateofExchange { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public string Amount
        {
            get
            {
                if (ForexAmount != null && RateofExchange != null)
                {
                    _Amount = $"{ForexAmount} @ {RateofExchange}";
                }
                else if (ForexAmount != null)
                {
                    _Amount = ForexAmount;
                }
                return _Amount;
            }
            set
            {
                if (value != null)
                {
                    if (value.ToString().Contains("="))
                    {
                        var s = value.ToString().Split('=');
                        var k = s[0].Split('@');
                        ForexAmount = k[0];
                        RateofExchange = k[1].Split()[2].Split('/')[0];
                        _Amount = s[1].Split()[2];
                    }
                    else
                    {
                        _Amount = value;
                    }
                }
                else
                {
                    _Amount = value;
                }

            }
        }

        [XmlElement(ElementName = "BILLALLOCATIONS.LIST")]
        public List<BillAllocations> BillAllocations { get; set; }

        [XmlElement(ElementName = "INVENTORYALLOCATIONS.LIST")]
        public List<InventoryAllocations> InventoryAllocations { get; set; }

        [XmlElement(ElementName = "CATEGORYALLOCATIONS.LIST")]
        public List<CostCategoryAllocations> CostCategoryAllocations { get; set; }

    }

    [XmlRoot(ElementName = "BILLALLOCATIONS.LIST")]
    public class BillAllocations
    {
        public BillAllocations()
        {
            _BillCP = new();
        }

        [XmlElement(ElementName = "BILLTYPE")]
        public string BillType { get; set; }

        [XmlElement(ElementName = "NAME")]
        public string Name { get; set; }

        private string _Amount;

        public string ForexAmount { get; set; }

        private BillCP _BillCP;

        

        [JsonIgnore]
        [XmlElement(ElementName = "BILLCREDITPERIOD")]
        public BillCP BillCP 
        {
            get { BillCreditPeriod = _BillCP.Days; return _BillCP; } set { _BillCP.Days = BillCreditPeriod; _BillCP = value; } }

        [XmlIgnore]
        public string BillCreditPeriod
        { get; set; }

        public string RateofExchange { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public string Amount
        {
            get
            {
                if (ForexAmount != null && RateofExchange != null)
                {
                    _Amount = $"{ForexAmount} @ {RateofExchange}";
                }
                else if (ForexAmount != null)
                {
                    _Amount = ForexAmount;
                }
                return _Amount;
            }
            set
            {
                if (value != null)
                {
                    if (value.ToString().Contains("="))
                    {
                        var s = value.ToString().Split('=');
                        var k = s[0].Split('@');
                        ForexAmount = k[0];
                        RateofExchange = k[1].Split()[2].Split('/')[0];
                        _Amount = s[1].Split()[2];
                    }
                    else
                    {
                        _Amount = value;
                    }
                }
                else
                {
                    _Amount = value;
                }

            }
        }

    }
    [XmlRoot(ElementName = "BILLCREDITPERIOD")]
    public class BillCP
    {
        [XmlAttribute(AttributeName = "JD")]
        public string JD { get; set; }

        private string _days;
        [XmlAttribute(AttributeName = "Days")]
        public string Days
        {
            get { return _days; }
            set { _days = value; }
        }

        [XmlText]
        public string TextValue
        {
            get { return _days; }
            set { _days = value; }
        }

    }

    [XmlRoot(ElementName = "INVENTORYALLOCATIONS.LIST")]
    public class InventoryAllocations
    {
        public InventoryAllocations()
        {
            BatchAllocations = new();
            CostCategoryAllocations = new();
        }


        [XmlElement(ElementName = "STOCKITEMNAME")]
        public string StockItemName { get; set; }

        [XmlElement(ElementName = "ISDEEMEDPOSITIVE")]
        public string DeemedPositive { get; set; }

        [XmlElement(ElementName = "RATE")]
        public string Rate { get; set; }

        [XmlElement(ElementName = "ACTUALQTY")]
        public string ActualQuantity { get; set; }

        [XmlElement(ElementName = "BILLEDQTY")]
        public string BilledQuantity { get; set; }

        private string _Amount;

        
        public string ForexAmount { get; set; }

        public string RateofExchange { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public string Amount
        {
            get
            {
                if (ForexAmount != null && RateofExchange != null)
                {
                    _Amount = $"{ForexAmount} @ {RateofExchange}";
                }
                else if (ForexAmount != null)
                {
                    _Amount = ForexAmount;
                }
                return _Amount;
            }
            set
            {
                if (value != null)
                {
                    if (value.ToString().Contains("="))
                    {
                        var s = value.ToString().Split('=');
                        var k = s[0].Split('@');
                        ForexAmount = k[0];
                        RateofExchange = k[1].Split()[2].Split('/')[0];
                        _Amount = s[1].Split()[2];
                    }
                    else
                    {
                        _Amount = value;
                    }
                }
                else
                {
                    _Amount = value;
                }

            }
        }

        [XmlElement(ElementName = "BATCHALLOCATIONS.LIST")]
        public List<BatchAllocations> BatchAllocations { get; set; }

        [XmlElement(ElementName = "CATEGORYALLOCATIONS.LIST")]
        public List<CostCategoryAllocations> CostCategoryAllocations { get; set; }

    }

    [XmlRoot(ElementName = "BATCHALLOCATIONS.LIST")]
    public class BatchAllocations //Godown Allocations
    {

        [XmlElement(ElementName = "TRACKINGNUMBER")]
        public string TrackingNo { get; set; }

        [XmlElement(ElementName = "ORDERNO")]
        public string OrderNo { get; set; }

        [XmlElement(ElementName = "GODOWNNAME")]
        public string GodownName { get; set; }

        [XmlElement(ElementName = "ACTUALQTY")]
        public string Quantity { get; set; }

        private string _Amount;

        public string ForexAmount { get; set; }

        public string RateofExchange { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public string Amount
        {
            get
            {
                if (ForexAmount != null && RateofExchange != null)
                {
                    _Amount = $"{ForexAmount} @ {RateofExchange}";
                }
                else if (ForexAmount != null)
                {
                    _Amount = ForexAmount;
                }
                return _Amount;
            }
            set
            {
                if (value != null)
                {
                    if (value.ToString().Contains("="))
                    {
                        var s = value.ToString().Split('=');
                        var k = s[0].Split('@');
                        ForexAmount = k[0];
                        RateofExchange = k[1].Split()[2].Split('/')[0];
                        _Amount = s[1].Split()[2];
                    }
                    else
                    {
                        _Amount = value;
                    }
                }
                else
                {
                    _Amount = value;
                }

            }
        }

    }

    [XmlRoot(ElementName = "CATEGORYALLOCATIONS.LIST")]
    public class CostCategoryAllocations
    {
        public CostCategoryAllocations()
        {
            CostCenterAllocations = new();
        }

        [XmlElement(ElementName = "CATEGORY")]
        public string CostCategoryName { get; set; }

        [XmlElement(ElementName = "COSTCENTREALLOCATIONS.LIST")]
        public List<CostCenterAllocations> CostCenterAllocations { get; set; }

    }
    [XmlRoot(ElementName = "COSTCENTREALLOCATIONS.LIST")]
    public class CostCenterAllocations
    {
        [XmlElement(ElementName = "NAME")]
        public string Name { get; set; }

        private string _Amount;

        public string ForexAmount { get; set; }

        public string RateofExchange { get; set; }

        [XmlElement(ElementName = "AMOUNT")]
        public string Amount
        {
            get
            {
                if (ForexAmount != null && RateofExchange != null)
                {
                    _Amount = $"{ForexAmount} @ {RateofExchange}";
                }
                else if (ForexAmount != null)
                {
                    _Amount = ForexAmount;
                }
                return _Amount;
            }
            set
            {
                if (value != null)
                {
                    if (value.ToString().Contains("="))
                    {
                        var s = value.ToString().Split('=');
                        var k = s[0].Split('@');
                        ForexAmount = k[0];
                        RateofExchange = k[1].Split()[2].Split('/')[0];
                        _Amount = s[1].Split()[2];
                    }
                    else
                    {
                        _Amount = value;
                    }
                }
                else
                {
                    _Amount = value;
                }

            }
        }


    }

    [XmlRoot(ElementName = "INVOICEDELNOTES.LIST")]
    public class DeliveryNotes
    {
        [XmlElement(ElementName = "BASICSHIPPINGDATE")]
        public string ShippingDate { get; set; }

        [XmlElement(ElementName = "BASICSHIPDELIVERYNOTE")]
        public string DeliveryNote { get; set; }
    }




    /// <summary>
    /// Voucher Message
    /// </summary>




    [XmlRoot(ElementName = "ENVELOPE")]
    public class VoucherEnvelope : TallyXmlJson
    {

        [XmlElement(ElementName = "HEADER")]
        public Header Header { get; set; }

        [XmlElement(ElementName = "BODY")]
        public VBody Body { get; set; } = new VBody();
    }

    [XmlRoot(ElementName = "BODY")]
    public class VBody
    {
        [XmlElement(ElementName = "DESC")]
        public Description Desc { get; set; } = new Description();

        [XmlElement(ElementName = "DATA")]
        public VData Data { get; set; } = new VData();
    }

    [XmlRoot(ElementName = "DATA")]
    public class VData
    {
        [XmlElement(ElementName = "TALLYMESSAGE")]
        public VoucherMessage Message { get; set; } = new VoucherMessage();
    }

    [XmlRoot(ElementName = "TALLYMESSAGE")]
    public class VoucherMessage
    {
        [XmlElement(ElementName = "VOUCHER")]
        public Voucher Voucher { get; set; }
    }


}


