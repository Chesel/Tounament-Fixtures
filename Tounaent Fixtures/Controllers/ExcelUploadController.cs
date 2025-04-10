﻿using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Tounaent_Fixtures.Controllers
{
    [Authorize]
    public class ExcelUploadController : Controller
    {
        public string? heading;
        public string? subheading;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upload(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                ViewBag.Error = "No file uploaded.";
                return View("Index");
            }

            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

                // Copy the uploaded file to the temporary location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                List<ExcelData> data = new List<ExcelData>();

            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheet(1);
                    var headersRow = worksheet.Row(4);

                    // Get required column indices
                    var requiredColumns = new Dictionary<string, int>
                    {
                        { "Student Name", GetColumnIndex(headersRow, "Student Name") },
                        { "Team", GetColumnIndex(headersRow, "Team") },
                        { "Height", GetColumnIndex(headersRow, "Height") },
                        { "Weight", GetColumnIndex(headersRow, "Weight") },
                        { "S.No", GetColumnIndex(headersRow, "S.No") },
                        { "DOB", GetColumnIndex(headersRow, "DOB") }
                    };

                    // Check if all required columns are found
                    if (requiredColumns.Values.Any(colIndex => colIndex == -1))
                    {
                        ViewBag.Error = "One or more required columns are missing.";
                        return View("Index");
                    }

                    // Process rows
                    foreach (var row in worksheet.RowsUsed().Skip(4))
                    {
                        if (row.Cell(requiredColumns["S.No"])?.Value != null)
                        {
                            data.Add(new ExcelData
                            {
                                Sno = Convert.ToInt32(row.Cell(requiredColumns["S.No"])?.Value.ToString().Trim()),
                                StudentName = row.Cell(requiredColumns["Student Name"])?.Value.ToString().Trim(),
                                Team = row.Cell(requiredColumns["Team"])?.Value.ToString().Trim(),
                                Height = row.Cell(requiredColumns["Height"])?.Value.ToString().Trim(),
                                Weight = Convert.ToInt32(row.Cell(requiredColumns["Weight"])?.Value.ToString().Trim()),
                                // DOB = DateTime.ParseExact(row.Cell(requiredColumns["DOB"])?.Value.ToString().Trim(), "dd-MM-yy", null)
                            });
                        }
                    }


                    HttpContext.Session.SetString("Heading", worksheet.Row(1).Cell(1)?.Value.ToString());
                    HttpContext.Session.SetString("SubHeading", worksheet.Row(3).Cell(1)?.Value.ToString());
                    //HttpContext.Session.SetString("DateTime", worksheet.Row(2).Cell(1)?.Value.ToString());
                    HttpContext.Session.SetString("Team", worksheet.Row(4).Cell(3)?.Value.ToString());
                    string cellValueAsString;
                    if (worksheet.Row(2).Cell(1)?.DataType == XLDataType.DateTime)
                    {
                        var dateValue = worksheet.Row(2).Cell(1)?.GetValue<DateTime>();
                        cellValueAsString = dateValue.HasValue ? dateValue.Value.ToString("yyyy-MM-dd") : " ";
                    }
                    else
                    {
                        cellValueAsString = worksheet.Row(2).Cell(1)?.GetValue<string>();
                    }
                    HttpContext.Session.SetString("DateTime",cellValueAsString);

                }

                var teamGroups = data.GroupBy(s => s.Team)
                                        .Select(g => g.ToList())
                                        .ToList();
                var shuffledStudents = new List<ExcelData>();
                while (teamGroups.Any(g => g.Count > 0))
                {
                    foreach (var group in teamGroups.ToList())
                    {
                        if (group.Count > 0)
                        {
                            shuffledStudents.Add(group.First());
                            group.RemoveAt(0); // Remove the student from the group

                            // Remove the group if it's empty
                            if (group.Count == 0)
                            {
                                teamGroups.Remove(group);
                            }
                        }
                    }
                }

                TempData["ViewName"] = GetViewName(shuffledStudents.Count);
                TempData["ShuffledStudents"] = JsonConvert.SerializeObject(shuffledStudents);

                return View("Data", data);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error reading Excel file: " + ex.Message;
                return View("Index");
            }
        }

        private int GetColumnIndex(IXLRow headersRow, string columnName)
        {
            foreach (var cell in headersRow.CellsUsed())
            {
                if (cell.Value.ToString().Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return cell.Address.ColumnNumber;
                }
            }
            return -1;
        }
        [HttpPost]
        public IActionResult NavigateToView(string viewName)
        {
            if (!string.IsNullOrEmpty(viewName))
            {
                var shuffledStudentsJson = TempData["ShuffledStudents"] as string;
                var shuffledStudents = JsonConvert.DeserializeObject<List<ExcelData>>(shuffledStudentsJson);
                ViewBag.students=shuffledStudents;
                ViewBag.Heading = HttpContext.Session.GetString("Heading");
                ViewBag.SubHeading = HttpContext.Session.GetString("SubHeading");
                ViewBag.date = HttpContext.Session.GetString("DateTime");
                ViewBag.Team = HttpContext.Session.GetString("Team");
                return View(viewName, shuffledStudents);
            }
            else
            {
                ViewBag.Error = "Error determining the view.";
                return View("Index");
            }
        }

        private string GetViewName(int count)
        {
            switch (count)
            {
                case 2:
                    return "2player";
                case 3:
                    return "3player";
                case 4:
                    return "4player";
                case 5:
                    return "5player";
                case 6:
                    return "6player";
                case 7:
                    return "7player";
                case 8:
                    return "8player";
                case 9:
                    return "9player";
                case 10:
                    return "10player";
                case 11:
                    return "11player";
                case 12:
                    return "12player";
                case 13:
                    return "13player";
                case 14:
                    return "14player";
                case 15:
                    return "15player";
                case 16:
                    return "16player";
                case 17:
                    return "17player";
                case 18:
                    return "18player";
                case 19:
                    return "19player";
                case 20:
                    return "20player";
                case 24:
                    return "24player";
                default:
                    return "Index";
            }
        }
    }


}