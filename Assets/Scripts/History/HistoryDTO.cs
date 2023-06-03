using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryDTO
{
    public int ReportId { get; set; }
    public int ChildrenAmount { get; set; }
    public int TotalStep { get; set; }
    public int TotalMoney { get; set; }
    public bool IsWin { get; set; }
    public int Score { get; set; }
    public int IncomePerMonth { get; set; }
    public int ExpensePerMonth { get; set; }
    public string NickName { get; set; }
    public string MatchId { get; set; }
    public string MatchTime { get; set; }
    public DateTime StartTime { get; set; }

}
