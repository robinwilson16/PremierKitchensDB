using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PremierKitchensDB.Shared
{
    public class Audit
    {
        public async static Task<bool> AddAuditRecord(ApplicationDbContext _context, char action, string tableName, string objectName, int objectID, string userID, string changeInfo)
        {
            //Only add audit record if there are changes
            if (!string.IsNullOrEmpty(changeInfo)) {
                AuditTrail auditTrail = new AuditTrail();

                int changeType;
                switch (action)
                {
                    case 'C':
                        changeType = 1;
                        break;
                    case 'E':
                        changeType = 2;
                        break;
                    case 'D':
                        changeType = 3;
                        break;
                    case 'V':
                        changeType = 4;
                        break;
                    default:
                        changeType = 0;
                        break;

                }

                auditTrail.TableName = tableName;
                auditTrail.ObjectID = objectID;
                auditTrail.WhereClause = objectName + " = " + objectID;
                auditTrail.RowDescription = objectName + " = " + objectID;
                auditTrail.ChangeInfo = changeInfo;
                auditTrail.ChangeType = changeType;
                auditTrail.UpdatedDate = DateTime.Now;
                auditTrail.UpdatedBy = userID;
                _context.AuditTrail.Add(auditTrail);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public static string WhatChanged(object originalObject, object modifiedObject, string previousChanges)
        {
            JsonResult originalObjectJ = new JsonResult(originalObject);
            JsonResult modifiedObjectJ = new JsonResult(modifiedObject);

            string originalValues = JsonConvert.SerializeObject(originalObjectJ.Value);
            string modifiedValues = JsonConvert.SerializeObject(modifiedObjectJ.Value);
            originalValues.Replace("{", "");
            originalValues.Replace("}", "");
            modifiedValues.Replace("{", "");
            modifiedValues.Replace("}", "");

            //Remove any commas in the text(only) which will cause issues
            string pattern = @"([a-zA-Z ])(,)([a-zA-Z ])";
            string substitution = @"$1|$3";
            RegexOptions options = RegexOptions.Multiline;
            Regex regex = new Regex(pattern, options);
            originalValues = regex.Replace(originalValues, substitution);
            modifiedValues = regex.Replace(modifiedValues, substitution);

            List<string> differences = new List<string>();
            List<string> originalList = originalValues.Split(',').Distinct().ToList();
            List<string> modifiedList = modifiedValues.Split(',').Distinct().ToList();

            string field;
            string oldValue;
            string newValue;
            for (int i = 0; i < modifiedList.Count; i++)
            {
                if (modifiedList.ElementAt(i) != originalList.ElementAt(i))
                {
                    field = modifiedList.ElementAt(i).Substring(0, modifiedList.ElementAt(i).IndexOf(":")).Trim('"');
                    oldValue = originalList.ElementAt(i).Substring(originalList.ElementAt(i).IndexOf(":") + 1, originalList.ElementAt(i).Length - originalList.ElementAt(i).IndexOf(":") - 1).Trim('"');
                    newValue = modifiedList.ElementAt(i).Substring(modifiedList.ElementAt(i).IndexOf(":") + 1, modifiedList.ElementAt(i).Length - modifiedList.ElementAt(i).IndexOf(":") - 1).Trim('"');

                    //Now change commas back to commas again
                    oldValue = oldValue.Replace("|", ",");
                    newValue = newValue.Replace("|", ",");

                    if (field != "UpdatedDate")
                    {
                        differences.Add(field + ": Changed from '" + oldValue + "' to '" + newValue + "'");
                    }
                }
            }

            string newChanges = String.Join(", ", differences.ToArray());

            if(!string.IsNullOrEmpty(previousChanges))
            {
                newChanges = previousChanges + ", " + newChanges;
            }

            //return originalValues + "|" + modifiedValues;
            return newChanges;
        }

        public static string ElementsChanged(object originalObject, object modifiedObject, string element, string previousChanges)
        {
            JsonResult originalObjectJ = new JsonResult(originalObject);
            JsonResult modifiedObjectJ = new JsonResult(modifiedObject);

            string originalValues = JsonConvert.SerializeObject(originalObjectJ.Value);
            string modifiedValues = JsonConvert.SerializeObject(modifiedObjectJ.Value);
            List<string> originalElements = new List<string>();
            List<string> modifiedElements = new List<string>();
            List<string> originalList = originalValues.Split(',').Distinct().ToList();
            List<string> modifiedList = modifiedValues.Split(',').Distinct().ToList();

            string field;
            string fieldVal;
            //Original elements
            for (int i = 0; i < originalList.Count; i++)
            {
                if (originalList.ElementAt(i).IndexOf(":") > 0)
                {
                    field = originalList.ElementAt(i).Substring(0, originalList.ElementAt(i).IndexOf(":")).Trim('"');
                    fieldVal = originalList.ElementAt(i).Substring(originalList.ElementAt(i).IndexOf(":") + 1, originalList.ElementAt(i).Length - originalList.ElementAt(i).IndexOf(":") - 1).Trim('[').Trim(']').Trim('{').Trim('}');

                    if (field == element)
                    {
                        originalElements.Add(fieldVal);
                    }
                }
            }

            //Modified elements
            for (int i = 0; i < modifiedList.Count; i++)
            {
                if (modifiedList.ElementAt(i).IndexOf(":") > 0)
                {
                    field = modifiedList.ElementAt(i).Substring(0, modifiedList.ElementAt(i).IndexOf(":")).Trim('"');
                    fieldVal = modifiedList.ElementAt(i).Substring(modifiedList.ElementAt(i).IndexOf(":") + 1, modifiedList.ElementAt(i).Length - modifiedList.ElementAt(i).IndexOf(":") - 1).Trim('[').Trim(']').Trim('{').Trim('}');

                    if (field == element)
                    {
                        modifiedElements.Add(fieldVal);
                    }
                }
            }

            string originalElementsStr = String.Join(", ", originalElements.ToArray());
            string modifiedElementsStr = String.Join(", ", modifiedElements.ToArray());
            string newChanges = "";

            if (modifiedElementsStr != originalElementsStr)
            {
                newChanges += element + ": Changed from '" + originalElementsStr + "' to '" + modifiedElementsStr + "'";
            }

            if (!string.IsNullOrEmpty(previousChanges))
            {
                newChanges = previousChanges + ", " + newChanges;
            }
            
            return newChanges;
        }
    }
}
