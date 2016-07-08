using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using WebApplication1.Utilities.EventSystem;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Contract
    {
       
        public int Id { get; set; }
        //David: Type of IntContractNum changed to String
        [DisplayName("Int. VertragsNr")] //4.1
        public string IntContractNum { get; set; }

        [DisplayName("Ext. VertragNr")] //4.2 Pflicht??
        public string ExtContractNum { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [DisplayName("Vertragsgegenstand")] //4.3 Pflicht
        public string Description { get; set; }

        [Required]
        [DisplayName("Unterzeichner")] //4.7
        public string SignerId { get; set; }
        [Required]
        [DisplayName("Sachlich Verantwortlicher")] //4.8
        public string OwnerId { get; set; }
        [DisplayName("Zugeordnete Abt.")] //4.9
        public Nullable<int> DepartmentId { get; set; }
        [DisplayName("Überwachende Abt.")] //4.10
        public Nullable<int> SupervisorDepartmentId { get; set; }
        [DisplayName("Vertragswert")] //4.11
        public Nullable<double> ContractValue { get; set; }
        [DisplayName("Kosten/Gsft-Jahr")] //4.12 ??
        public Nullable<double> AnnualValue { get; set; }
        //Further Characteristics //4.13
        [DisplayName("Im Voraus zahlbar")]
        public Nullable<bool> PrePayable { get; set; }
        [DisplayName("Variable Kosten")]
        public Nullable<bool> VarPayable { get; set; }
        [DisplayName("Zulassung von Preisanpassung")]
        public Nullable<bool> Adaptable { get; set; }
        //characteristics:end
        [DisplayName("Zahlungsbeginn")] //4.14
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)] //Ober:Edit german Date format
        public Nullable<DateTime> PaymentBegin { get; set; }
        [DisplayName("Zahlungsintervall")] //4.15
        public Nullable<int> PaymentInterval { get; set; }

        [DisplayName("Vertragsbeginn")] //4.19 Pflicht
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)] //Ober:Edit german Date format
        public Nullable<DateTime> ContractBegin { get; set; }
        [DisplayName("Kündigungsfrist")] //4.20
        public Period CancellationPeriod { get; set; }
        [DisplayName("Kündigungstermin")] //4.21
        public Nullable<DateTime> CancellationDate { get; set; }
        //Contract Duration in months
        [DisplayName("Min. Vertragsdauer")] //4.22
        [Range(12, int.MaxValue)]
        public Nullable<int> MinContractDuration { get; set; }
        [DisplayName("Vertragsende")] //4.23
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)] //Ober:Edit german Date format
        public Nullable<DateTime> ContractEnd { get; set; }
        [DisplayName("stillschweigende Verlängerung")] //4.24
        public Nullable<int> AutoExtension { get; set; }
        //Erinnerung?? //4.25
        //Escalation?? //4.26
        [DisplayName("MwSt")] //4.27
        public Nullable<double> Tax { get; set; }
        [DisplayName("Bemerkungen")] //4.28
        public string Remarks { get; set; }


        //Extra
        [DisplayName("Ist Rahmenvertrag")]
        public Nullable<bool> IsFrameContract { get; set; }

        //Not relevant
        [DisplayName("Dispatcher")]
        public string DispatcherId { get; set; }
            
        //virtua: multiple Objects
        [DisplayName("Unterzeichner")] //4.7
        public virtual ContractUser Signer { get; set; }
        [DisplayName("Sachlich Verantwortlicher")] //4.8
        public virtual ContractUser Owner { get; set; }
        [DisplayName("Zugeordnete Abt.")] //4.9
        public virtual Department Department { get; set; }
        [DisplayName("Überwachende Abt.")] //4.10
        public virtual Department SupervisorDepartment { get; set; }
        //virtual: multple Objects not relevant
        [DisplayName("Dispatcher")]
        public virtual ContractUser Dispatcher { get; set; }
        

        //virtual
        [DisplayName("Vertragskategorie")] //4.4 Pflicht
        public virtual ContractType ContractType { get; set; }
        [DisplayName("Unterkategorie")] //4.5
        public virtual ContractSubType ContractSubType { get; set; }
        [DisplayName("Vertragsart")] //4.6 Pflicht
        public virtual ContractKind ContractKind { get; set; }
        //Multple of same type

        [DisplayName("Kostenstellen")] //4.16
        public virtual ICollection<ContractCostCenter_Relation> ContractCostCenter_Relations { get; set; }
        [DisplayName("Kostenart")] //4.17
        public virtual CostKind CostKind { get; set; }
        [DisplayName("Vertragspartner")] //4.18
        public virtual ContractPartner ContractPartner { get; set; }
        
        [DisplayName("Vertragsstatus")] //4.29
        public virtual ContractStatus ContractStatus { get; set; }
           
        [DisplayName("Ablageort")] //4.30
        public virtual PhysicalDocAddress PhysicalDocAddress { get; set; }

        //virtual Extra
        public virtual ICollection<ContractFile> ContractFiles { get; set; }
        //Moses : ContractTasks
        public virtual ICollection<ContractTask> ContractTasks { get; set; }
        public virtual ICollection<VarPayment> VarPayments { get; set; }

        //David: Framecontract
        public int? FrameContractId { get; set; }

        public virtual Contract FrameContract { get; set; }
        public virtual ICollection<Contract> SubContracts { get; set; }

        //David:
        [DisplayName("Vertragslaufzeitoptionen")] //Ober
        public Nullable<ContractRuntimeTypes> RuntimeType { get; set; } //Ober set nullable

        //David: Events for Tasks ***************************************************************************

        //Events for the DispatcherTask ------------------------------------------------ 1
        public delegate void DispatcherTaskEventHandler(object source, EventArgs args);
        public event DispatcherTaskEventHandler DispatcherTask;

        protected virtual void OnDispatcherTask()
        {
            if(DispatcherTask != null)
            {
                DispatcherTask(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerDispatcherTaskEvent()
        {
            this.DispatcherTask += EventUtility.OnDispatcherNeeded;
            this.DispatcherTask += EventUtility.MessengerOnContractCreated;
            OnDispatcherTask();
            this.DispatcherTask -= EventUtility.OnDispatcherNeeded;
            this.DispatcherTask -= EventUtility.MessengerOnContractCreated;
        }


        //Events when the dispatcher is set ---------------------------------------------- 2
        public delegate void DispatcherSetEventHandler(object source, EventArgs args);
        public event DispatcherSetEventHandler DispatcherSet;

        protected virtual void OnDispatcherSet()
        {
            if (DispatcherSet != null)
            {
                DispatcherSet(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerDispatcherSetEvent()
        {
            this.DispatcherSet += EventUtility.OnDispatcherSet;
            this.DispatcherSet += MailUtility.OnDispatcherSet;
            OnDispatcherSet();
            this.DispatcherSet -= EventUtility.OnDispatcherSet;
            this.DispatcherSet -= MailUtility.OnDispatcherSet;
        }

        //Event for Contract-Completion Task ---------------------------------------------- 3
        public delegate void ContractCompletionEventHandler(object source, EventArgs args);
        public event ContractCompletionEventHandler ContractToBeCompleted;

        protected virtual void OnContractToBeCompleted()
        {
            if (ContractToBeCompleted != null)
            {
                ContractToBeCompleted(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerContractToBeCompletedEvent()
        {
            this.ContractToBeCompleted += EventUtility.OnContractToBeCompleted;
            OnContractToBeCompleted();
            this.ContractToBeCompleted -= EventUtility.OnContractToBeCompleted;
        }

        //Event for "Files To Be Added" Task ---------------------------------------------- 4
        public delegate void FilesToBeAddedEventHandler(object source, EventArgs args);
        public event FilesToBeAddedEventHandler FilesToBeAdded;

        protected virtual void OnFilesToBeAdded()
        {
            if (FilesToBeAdded != null)
            {
                FilesToBeAdded(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerFilesToBeAddedEvent()
        {
            this.FilesToBeAdded += EventUtility.OnFilesToBeAdded;
            OnFilesToBeAdded();
            this.FilesToBeAdded -= EventUtility.OnFilesToBeAdded;
        }

        //Event when Contract Information is complete ------------------------------------ 5
        public delegate void ContractCompletedEventHandler(object source, EventArgs args);
        public event ContractCompletedEventHandler ContractCompleted;

        protected virtual void OnContractCompleted()
        {
            if (ContractCompleted != null)
            {
                ContractCompleted(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerContractCompletedEvent()
        {
            this.ContractCompleted += EventUtility.OnContractCompleted;
            OnContractCompleted();
            this.ContractCompleted -= EventUtility.OnContractCompleted;
        }

        //Event when Contract-File is added --------------------------------------------- 6
        public delegate void FilesAddedEventHandler(object source, EventArgs args);
        public event FilesAddedEventHandler FilesAdded;

        protected virtual void OnFilesAdded()
        {
            if (FilesAdded != null)
            {
                FilesAdded(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerFilesAddedEvent()
        {
            this.FilesAdded += EventUtility.OnFilesAdded;
            OnFilesAdded();
            this.FilesAdded -= EventUtility.OnFilesAdded;
        }

        //David: Events for Tasks ***********************************************************************ENDE

    }

    //David: Nur als Info zur Art der Laufzeit des Vertrags
    public enum ContractRuntimeTypes
    {
        //wird in db als int gespeichert
        //Ober changed order unlimited with fixedTermWithP...
        [Display(Name = "feste Laufzeit")]
        [Description("feste Laufzeit")]
        fixedTerm = 1,  //= 1 feste Laufzeit  
        [Display(Name = "feste Laufzeit mit vorz. Kündigung")]
        [Description("feste Laufzeit mit vorz. Kündigung")]
        fixedTermWithPrematureTermination = 2,  //= 2 feste Laufzeit mit vorz. Kuendigung
        [Display(Name = "unbefristet mit stillschweigender Verlängerung")]
        [Description("unbefristet mit stillschweigender Verlängerung")]
        unlimited = 3  //= 3 unbefristet mit stillschweigender Verlaengerung
    }

}