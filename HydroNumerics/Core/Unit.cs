#region Copyright
/*
* Copyright (c) 2010, Jan Gregersen (HydroInform) & Jacob Gudbjerg
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the names of Jan Gregersen (HydroInform) & Jacob Gudbjerg nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY "Jan Gregersen (HydroInform) & Jacob Gudbjerg" ``AS IS'' AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL "Jan Gregersen (HydroInform) & Jacob Gudbjerg" BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace HydroNumerics.Core
{

	/// <summary>
	/// The Unit class defines a unit for a quantity.
    /// <para>This is a trivial implementation of OpenMI.Standard.IUnit, refer there for further details.</para>
    /// </summary>
	[Serializable]
  [DataContract]
	public class Unit
	{
    [DataMember]
		private string _description="";
    [DataMember]
    private string _id = "";
    [DataMember]
    private double _conversionFactor = 1;
    [DataMember]
    private double _conversionOffset = 0;
    [DataMember]
    private Dimension _dimension;

		/// <summary>
		/// Constructor
		/// </summary>
		public Unit()
		{
            _dimension = new Dimension();
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="source">The unit to copy</param>
		public Unit(Unit source)
		{
			Description = source.Description;
			ID = source.ID;
			ConversionFactorToSI = source.ConversionFactorToSI;
			OffSetToSI = source.OffSetToSI;
            _dimension = new Dimension(source.Dimension);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ID">ID</param>
		/// <param name="conversionFactor">Conversion factor to SI</param>
		/// <param name="conversionOffset">Conversion offset to SI</param>
		public Unit(string ID, double conversionFactor, double conversionOffset):this()
		{
            if (conversionFactor == 0)
            {
                throw new Exception("The unit conversion factor may not be equal to zero");
            }
			_id = ID;
			_conversionFactor = conversionFactor;
			_conversionOffset = conversionOffset;
			_description = "";
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ID">ID</param>
		/// <param name="conversionFactor">Conversion factor to SI</param>
		/// <param name="conversionOffset">Conversion offset to SI</param>
		/// <param name="description">Description</param>
		public Unit(string ID, double conversionFactor, double conversionOffset,string description):this()
		{
            if (conversionFactor == 0)
            {
                throw new Exception("The unit conversion factor may not be equal to zero");
            }
			_id = ID;
			_conversionFactor = conversionFactor;
			_conversionOffset = conversionOffset;
			_description = description;
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="conversionFactor">Conversion factor to SI</param>
        /// <param name="conversionOffset">Conversion offset to SI</param>
        /// <param name="description">Description</param>
        /// <param name="dimension">Dimention</param>
        public Unit(string ID, double conversionFactor, double conversionOffset, string description, Dimension dimension): this()
        {
            if (conversionFactor == 0)
            {
                throw new Exception("The unit conversion factor may not be equal to zero");
            }
            _id = ID;
            _conversionFactor = conversionFactor;
            _conversionOffset = conversionOffset;
            _description = description;
            _dimension = dimension;
        }

		/// <summary>
		/// Getter and setter for description
		/// </summary>
        //[XmlAttribute]
        public string Description
		{
			get { return _description;}
			set
			{
				_description = value;
			}
		}

        public Dimension Dimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }

		/// <summary>
        /// Conversion factor to SI ('A' in: SI-value = A * value + B)
		/// </summary>
        //[XmlAttribute]
        public double ConversionFactorToSI
		{
			get {return _conversionFactor;}
			set
			{
                if (value == 0)
                {
                    throw new Exception("The unit conversion factor may not be equal to zero");
                }
				_conversionFactor = value;
			}
		}

		/// <summary>
        /// OffSet to SI ('B' in: SI-value = A * value + B)
		/// </summary>
        //[XmlAttribute]
        public double OffSetToSI
		{
			get {return _conversionOffset;}
			set
			{
				_conversionOffset = value;
			}
		}

		/// <summary>
		/// Getter and setter for ID
		/// </summary>
        //[XmlAttribute]
        public string ID
		{
			get {return _id;}
			set
			{
				_id = value;
			}
		}

		///<summary>
		/// Check if the current instance equals another instance of this class.
		///</summary>
		///<param name="obj">The instance to compare the current instance with.</param>
		///<returns><code>true</code> if the instances are the same instance or have the same content.</returns>
		public override bool Equals(Object obj) 
		{
			if (obj == null || GetType() != obj.GetType()) 
				return false;
			Unit u = (Unit) obj;
			if (!ID.Equals(u.ID))
				return false;
			if (!Description.Equals(u.Description))
				return false;
			if (!ConversionFactorToSI.Equals(u.ConversionFactorToSI))
				return false;
			if (!OffSetToSI.Equals(u.OffSetToSI))
				return false;
            if (!this.Dimension.Equals(u.Dimension))
                return false;
			return true;
		}


        public XmlElement ToXml(XmlDocument xmlDocument)
        {
            XmlElement xmlUnit = xmlDocument.CreateElement("Unit");
            xmlUnit.SetAttribute("ID", this._id);
            xmlUnit.SetAttribute("Description", this._description);
            xmlUnit.SetAttribute("SiConversionFactor", this._conversionFactor.ToString());
            xmlUnit.SetAttribute("SiOffset", this._conversionOffset.ToString());
            return xmlUnit;
        }

        
        /// <summary>
        /// Converts the value provided as argument for this method to this unit. The value
        /// provided in the argument must be in SI unit.
        /// </summary>
        /// <param name="value">Values to convert (must be in SI unit)</param>
        /// <returns>Value in this unit</returns>
        public double FromSiToThisUnit(double valueInSiUnit)
        {
            return (valueInSiUnit - OffSetToSI) / ConversionFactorToSI;
        }

        /// <summary>
        /// Converts the value provided as argument for this method to SI unit. The value provided
        /// must be in the unit af defined by this unit.
        /// </summary>
        /// <param name="value">Value (must be in the unit as defined by this unit)</param>
        /// <returns>The value converted to SI unit</returns>
        public double ToSiUnit(double valueInThisUnit)
        {
            return valueInThisUnit * ConversionFactorToSI + OffSetToSI;
        }

        /// <summary>
        /// Converts the value provided as argument for this method to this unit. The provided 
        /// value must be represented in the unit provided as argument for this method.
        /// </summary>
        /// <param name="value">The value to convert (must be in the unit as defined in the fromUnit argument)</param>
        /// <param name="fromUnit">The value converted to this unit</param>
        /// <returns></returns>
        public double FromUnitToThisUnit(double valueInFromUnit, Unit fromUnit)
        {
            return FromSiToThisUnit(fromUnit.ToSiUnit(valueInFromUnit));
        }

        /// <summary>
        /// Converts the value provided as argument for this method from this unit to the unit
        /// provided in the argument list. The value provided must be defined by this unit.
        /// </summary>
        /// <param name="value">value (in the unit of this unit)</param>
        /// <param name="toUnit">the unit to which the value is converted</param>
        /// <returns></returns>
        public double FromThisUnitToUnit(double valueInThisUnit, Unit toUnit)
        {
            double xSI = ToSiUnit(valueInThisUnit);
            return (xSI - toUnit.OffSetToSI) / toUnit.ConversionFactorToSI;
        }
	}
}
 