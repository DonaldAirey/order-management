// <copyright file="DebtWorkingOrder.cs" company="Teraque, Inc.">
//     Copyright © 2013 - Teraque, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace Teraque.AssetNetwork.Windows
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using Teraque.AssetNetwork;

    /// <summary>
    /// The Model View for generic Working Orders.
    /// </summary>
    public class DebtWorkingOrder : WorkingOrder
    {
        /// <summary>
        /// Beta Adjusted Key Rate Duration 10 Year.
        /// </summary>
        private Decimal betaAdjustedKrd10YearField;

        /// <summary>
        /// Beta Adjusted Key Rate Duration 20 Year.
        /// </summary>
        private Decimal betaAdjustedKrd20YearField;

        /// <summary>
        /// Beta Adjusted Key Rate Duration 2 Year.
        /// </summary>
        private Decimal betaAdjustedKrd2YearField;

        /// <summary>
        /// Beta Adjusted Key Rate Duration 30 Year.
        /// </summary>
        private Decimal betaAdjustedKrd30YearField;

        /// <summary>
        /// Beta Adjusted Key Rate Duration 5 Year.
        /// </summary>
        private Decimal betaAdjustedKrd5YearField;

        /// <summary>
        /// Beta Adjusted Key Rate Duration 6 Month.
        /// </summary>
        private Decimal betaAdjustedKrd6MonthField;

        /// <summary>
        /// The instrument's coupon.
        /// </summary>
        private Decimal couponField;

        /// <summary>
        /// Credit Rating 0 Scale.
        /// </summary>
        private String creditRating0ScaleField;

        /// <summary>
        /// Credit Rating 0 Value.
        /// </summary>
        private Int32 creditRating0ValueField;

        /// <summary>
        /// Credit Rating 1 Scale.
        /// </summary>
        private String creditRating1ScaleField;

        /// <summary>
        /// Credit Rating 1 Value.
        /// </summary>
        private Int32 creditRating1ValueField;

        /// <summary>
        /// Credit Rating 2 Scale.
        /// </summary>
        private String creditRating2ScaleField;

        /// <summary>
        /// Credit Rating 2 Value.
        /// </summary>
        private Int32 creditRating2ValueField;

        /// <summary>
        /// Current Yield.
        /// </summary>
        private Decimal currentYieldField;

        /// <summary>
        /// The instrument's maturity date.
        /// </summary>
        private DateTime maturityDateField;

        /// <summary>
        /// MSSA Bucket.
        /// </summary>
        private String mssaBucketField;

        /// <summary>
        /// Subordinate Type.
        /// </summary>
        private String subordinateTypeField;

        /// <summary>
        /// Initializes a new instance of the DebtWorkingOrder class from a record in the data model.
        /// </summary>
        /// <param name="workingOrderRow">The working order record in the data model.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Reviewed")]
        public DebtWorkingOrder(DataModel.WorkingOrderRow workingOrderRow)
            : base(workingOrderRow)
        {
            // The new instance is initialized with a copy of the data from the data model.
            this.Copy(workingOrderRow);
        }

        /// <summary>
        /// Gets or sets the Beta Adjusted Key Rate Duration 10 Year.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Krd", Justification = "Reviewed")]
        public Decimal BetaAdjustedKrd10Year
        {
            get
            {
                return this.betaAdjustedKrd10YearField;
            }

            set
            {
                {
                    if (this.betaAdjustedKrd10YearField != value)
                    {
                        this.betaAdjustedKrd10YearField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("BetaAdjustedKrd10Year"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the Beta Adjusted Key Rate Duration 20 Year.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Krd", Justification = "Reviewed")]
        public Decimal BetaAdjustedKrd20Year
        {
            get
            {
                return this.betaAdjustedKrd20YearField;
            }

            set
            {
                {
                    if (this.betaAdjustedKrd20YearField != value)
                    {
                        this.betaAdjustedKrd20YearField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("BetaAdjustedKrd20Year"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the Beta Adjusted Key Rate Duration 2 Year.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Krd", Justification = "Reviewed")]
        public Decimal BetaAdjustedKrd2Year
        {
            get
            {
                return this.betaAdjustedKrd2YearField;
            }

            set
            {
                {
                    if (this.betaAdjustedKrd2YearField != value)
                    {
                        this.betaAdjustedKrd2YearField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("BetaAdjustedKrd2Year"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the Beta Adjusted Key Rate Duration 30 Year.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Krd", Justification = "Reviewed")]
        public Decimal BetaAdjustedKrd30Year
        {
            get
            {
                return this.betaAdjustedKrd30YearField;
            }

            set
            {
                {
                    if (this.betaAdjustedKrd30YearField != value)
                    {
                        this.betaAdjustedKrd30YearField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("BetaAdjustedKrd30Year"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the Beta Adjusted Key Rate Duration 30 Year.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Krd", Justification = "Reviewed")]
        public Decimal BetaAdjustedKrd5Year
        {
            get
            {
                return this.betaAdjustedKrd5YearField;
            }

            set
            {
                {
                    if (this.betaAdjustedKrd5YearField != value)
                    {
                        this.betaAdjustedKrd5YearField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("BetaAdjustedKrd5Year"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the Beta Adjusted Key Rate Duration 30 Year.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Krd", Justification = "Reviewed")]
        public Decimal BetaAdjustedKrd6Month
        {
            get
            {
                return this.betaAdjustedKrd6MonthField;
            }

            set
            {
                {
                    if (this.betaAdjustedKrd6MonthField != value)
                    {
                        this.betaAdjustedKrd6MonthField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("BetaAdjustedKrd6Month"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the coupon.
        /// </summary>
        public Decimal Coupon
        {
            get
            {
                return this.couponField;
            }

            set
            {
                {
                    if (this.couponField != value)
                    {
                        this.couponField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("Coupon"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the creditRating0Value.
        /// </summary>
        public Int32 CreditRating0Value
        {
            get
            {
                return this.creditRating0ValueField;
            }

            set
            {
                {
                    if (this.creditRating0ValueField != value)
                    {
                        this.creditRating0ValueField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("CreditRating0Value"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the creditRating0Scale.
        /// </summary>
        public String CreditRating0Scale
        {
            get
            {
                return this.creditRating0ScaleField;
            }

            set
            {
                {
                    if (this.creditRating0ScaleField != value)
                    {
                        this.creditRating0ScaleField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("CreditRating0Scale"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the creditRating1Value.
        /// </summary>
        public Int32 CreditRating1Value
        {
            get
            {
                return this.creditRating1ValueField;
            }

            set
            {
                {
                    if (this.creditRating1ValueField != value)
                    {
                        this.creditRating1ValueField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("CreditRating1Value"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the creditRating1Scale.
        /// </summary>
        public String CreditRating1Scale
        {
            get
            {
                return this.creditRating1ScaleField;
            }

            set
            {
                {
                    if (this.creditRating1ScaleField != value)
                    {
                        this.creditRating1ScaleField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("CreditRating1Scale"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the creditRating2Value.
        /// </summary>
        public Int32 CreditRating2Value
        {
            get
            {
                return this.creditRating2ValueField;
            }

            set
            {
                {
                    if (this.creditRating2ValueField != value)
                    {
                        this.creditRating2ValueField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("CreditRating2Value"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the creditRating2Scale.
        /// </summary>
        public String CreditRating2Scale
        {
            get
            {
                return this.creditRating2ScaleField;
            }

            set
            {
                {
                    if (this.creditRating2ScaleField != value)
                    {
                        this.creditRating2ScaleField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("CreditRating2Scale"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the currentYield.
        /// </summary>
        public Decimal CurrentYield
        {
            get
            {
                return this.currentYieldField;
            }

            set
            {
                {
                    if (this.currentYieldField != value)
                    {
                        this.currentYieldField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentYield"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maturity date.
        /// </summary>
        public DateTime MaturityDate
        {
            get
            {
                return this.maturityDateField;
            }

            set
            {
                {
                    if (this.maturityDateField != value)
                    {
                        this.maturityDateField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("MaturityDate"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maturity date.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mssa", Justification = "Reviewed")]
        public String MssaBucket
        {
            get
            {
                return this.mssaBucketField;
            }

            set
            {
                {
                    if (this.mssaBucketField != value)
                    {
                        this.mssaBucketField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("MssaBucket"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maturity date.
        /// </summary>
        public String SubordinateType
        {
            get
            {
                return this.subordinateTypeField;
            }

            set
            {
                {
                    if (this.subordinateTypeField != value)
                    {
                        this.subordinateTypeField = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("SubordinateType"));
                    }
                }
            }
        }

        /// <summary>
        /// Copy the values from the data model.
        /// </summary>
        /// <param name="workingOrderRow">The data model row that is the source of the data.</param>
        public override void Copy(DataModel.WorkingOrderRow workingOrderRow)
        {
            // Validate the parameters.
            if (workingOrderRow == null)
            {
                throw new ArgumentNullException("workingOrderRow");
            }

            // Allow the base class to copy the core of the working order.
            base.Copy(workingOrderRow);

            // The working order requires information from the Debt Security being ordered.
            foreach (DataModel.DebtRow debtRow in workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.GetDebtRowsByFK_Security_Debt_DebtId())
            {
                // Copy the scalar that are specific to the Debt instruments directly from the data model.
                this.MaturityDate = debtRow == null ? DateTime.MinValue : debtRow.MaturityDate;

                foreach (DataModel.DebtAttributeRow debtAttributeRow in debtRow.GetDebtAttributeRows())
                {
                    this.BetaAdjustedKrd10Year = debtAttributeRow.BetaAdjustedKrd10Year;
                    this.BetaAdjustedKrd20Year = debtAttributeRow.BetaAdjustedKrd20Year;
                    this.BetaAdjustedKrd2Year = debtAttributeRow.BetaAdjustedKrd2Year;
                    this.BetaAdjustedKrd30Year = debtAttributeRow.BetaAdjustedKrd30Year;
                    this.BetaAdjustedKrd5Year = debtAttributeRow.BetaAdjustedKrd5Year;
                    this.BetaAdjustedKrd6Month = debtAttributeRow.BetaAdjustedKrd6Month;
                    this.Coupon = debtAttributeRow.Coupon;
                    this.CreditRating0Value = debtAttributeRow.CreditRatingRowByFK_CreditRating_DebtAttribute_CreditRatingId0.Value;
                    this.CreditRating0Scale = debtAttributeRow.CreditRatingRowByFK_CreditRating_DebtAttribute_CreditRatingId0.Scale;
                    this.CreditRating1Value = debtAttributeRow.CreditRatingRowByFK_CreditRating_DebtAttribute_CreditRatingId1.Value;
                    this.CreditRating1Scale = debtAttributeRow.CreditRatingRowByFK_CreditRating_DebtAttribute_CreditRatingId1.Scale;
                    this.CreditRating2Value = debtAttributeRow.CreditRatingRowByFK_CreditRating_DebtAttribute_CreditRatingId2.Value;
                    this.CreditRating2Scale = debtAttributeRow.CreditRatingRowByFK_CreditRating_DebtAttribute_CreditRatingId2.Scale;
                    this.CurrentYield = debtAttributeRow.CurrentYield;
                    this.MssaBucket = debtAttributeRow.MssaBucketRow.Name;
                    this.SubordinateType = debtAttributeRow.SubordinateTypeRow.Description;
                }
            }
        }
    }
}
