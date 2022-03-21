// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 17/05/2021 12:04 PM
// Created On: NS00J002003DE01

namespace TunaTK
{
	public abstract class Validator<TO_VALIDATE> 
	{
		public abstract bool Validate(TO_VALIDATE _value);
	}
}