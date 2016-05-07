using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;

namespace DpControl.Domain.Repository
{
    public static class Utilities
    {
        public static int ConverRelationIdToInt(string relationId)
        {
            try
            {
                return System.Convert.ToInt32(relationId);
            }
            catch
            {
                throw new ExpectException("Wrong Id format :" + relationId);
            }
        }
    }
}
