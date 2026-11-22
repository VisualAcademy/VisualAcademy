using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualAcademy.Models.AzuntDashboard;

public sealed record DashboardNavigationItem(
    string Key, string Text, string Icon, string Href,
    IReadOnlyList<DashboardNavigationItem>? Children = null,
    bool ExpandedByDefault = false, string? Group = null);

public sealed class DashboardNavigationNodeViewModel
{
    public string Key { get; init; } = "";
    public string Text { get; init; } = "";
    public string Icon { get; init; } = "";
    public string Href { get; init; } = "";
    public int Depth { get; init; }
    public bool IsCurrent { get; init; }
    public bool ContainsCurrent { get; init; }
    public bool IsExpanded { get; init; }
    public string? Group { get; init; }
    public IReadOnlyList<DashboardNavigationNodeViewModel> Children { get; init; } = [];
}

public sealed class DashboardNavigationViewModel
{
    public IReadOnlyList<DashboardNavigationNodeViewModel> Items { get; init; } = [];
}

/// <summary>
/// Azunt starter navigation plus the complete original VisualAcademy Blazor sidebar.
/// Original link paths and hierarchy are retained; no business routes or auth policies are changed.
/// </summary>
public static class DashboardNavigationCatalog
{
    private static readonly IReadOnlyList<DashboardNavigationItem> Navigation =
    [
        new("overview", "Overview", "home", "/dashboard", Group: "AZUNT STARTER"),
        new("resources", "Resources", "resources", "/dashboard/resources", [
            new("app-services", "App services", "app-service", "/dashboard/resources/app-services"),
            new("databases", "Databases", "database", "/dashboard/resources/databases"),
            new("storage", "Storage", "storage", "/dashboard/resources/storage")
        ], Group: "AZUNT STARTER"),
        new("management", "Management", "settings", "/dashboard/management", [
            new("identity", "Identity", "identity", "/dashboard/management/identity", [
                new("users", "Users", "users", "/dashboard/management/identity/users"),
                new("roles", "Roles", "roles", "/dashboard/management/identity/roles")
            ]),
            new("tenants", "Tenants", "tenants", "/dashboard/management/tenants")
        ], Group: "AZUNT MANAGEMENT"),
        new("monitoring", "Monitoring", "monitoring", "/dashboard/monitoring", [
            new("activity", "Activity log", "activity", "/dashboard/monitoring/activity"),
            new("health", "Service health", "health", "/dashboard/monitoring/health")
        ], Group: "AZUNT MANAGEMENT"),
        new("documentation", "Documentation", "docs", "/docs/", Group: "AZUNT LINKS"),
        new("protected-docs", "Protected docs", "shield", "/docs/private/", Group: "AZUNT LINKS"),
        new("account", "Account", "account", "/Identity/Account/Manage", Group: "AZUNT LINKS"),
        new("va-nav-1", "Home", "resources", "/dashboard", Group: "VISUALACADEMY · CORE"),
        new("va-nav-2", "Counter", "dashboard", "/counter", Group: "VISUALACADEMY · CORE"),
        new("va-nav-3", "Fetch data", "database", "/fetchdata", Group: "VISUALACADEMY · CORE"),
        new("va-nav-4", "Blazor App", "resources", "/TodoListInMemory", [
            new("va-nav-5", "TodoList", "activity", "/TodoListInMemory", [
                new("va-nav-6", "TodoListInMemory", "activity", "/TodoListInMemory"),
                new("va-nav-7", "TodoListInMemory", "activity", "/TodoListInMemory")
            ]),
            new("va-nav-8", "Bonus", "resources", "/Bonus"),
            new("va-nav-9", "CRUD", "resources", "/TextTemplates", [
                new("va-nav-10", "TextTemplates", "resources", "/TextTemplates")
            ])
        ], Group: "VISUALACADEMY · CORE"),
        new("va-nav-11", "Fundamentals", "courses", "/Candidates", [
            new("va-nav-12", "Candidates", "users", "/Candidates", [
                new("va-nav-13", "Candidates", "users", "/Candidates"),
                new("va-nav-14", "강의 소스 GitHub", "resources", "https://github.com/VisualAcademy/Buffet")
            ]),
            new("va-nav-15", "CandidatesNames", "users", "/Candidates/CandidatesNames/Manage"),
            new("va-nav-16", "CandidatesIncomes", "users", "/Candidates/CandidatesIncomes/Manage"),
            new("va-nav-17", "Noodles", "resources", "/Buffets/Noodles"),
            new("va-nav-18", "Add Noodle", "resources", "/Buffets/Noodles/Create")
        ], Group: "VISUALACADEMY · CORE"),
        new("va-nav-19", "게시판(Articles)", "docs", "/Articles", Group: "VISUALACADEMY · CORE"),
        new("va-nav-20", "Components", "app-service", "/DulDropDownListServerFiltering", [
            new("va-nav-21", "Filter DropDownList", "resources", "/DulDropDownListServerFiltering")
        ], Group: "VISUALACADEMY · CORE"),
        new("va-nav-22", "영업 (Sales)", "products", "/Accounts", [
            new("va-nav-23", "고객 (Customers)", "users", "/Accounts", [
                new("va-nav-24", "고객 (Accounts)", "users", "/Accounts"),
                new("va-nav-25", "연락처 (Contacts)", "resources", "/Contacts")
            ]),
            new("va-nav-26", "영업 (Sales)", "products", "/Leads", [
                new("va-nav-27", "잠재 고객 (Leads)", "users", "/Leads"),
                new("va-nav-28", "영업 기회 (Opportunities)", "products", "/Opportunities"),
                new("va-nav-29", "경쟁 업체 (Competitors)", "resources", "/Competitors"),
                new("va-nav-30", "견적 (Quotes)", "resources", "/Quotes"),
                new("va-nav-31", "주문 (Orders)", "resources", "/Orders"),
                new("va-nav-32", "송장 (Invoices)", "resources", "/Invoices")
            ]),
            new("va-nav-33", "참고자료 (Reference)", "resources", "/Products", [
                new("va-nav-34", "제품 (Products)", "resources", "/Products"),
                new("va-nav-35", "가격표 (Price Lists)", "resources", "/PriceLists"),
                new("va-nav-36", "단위 (Units)", "resources", "/Units"),
                new("va-nav-37", "영업 구역 (Territories)", "products", "/Territories")
            ])
        ], Group: "VISUALACADEMY · BUSINESS"),
        new("va-nav-38", "마케팅 (Marketing)", "monitoring", "/Segments", [
            new("va-nav-39", "고객 대상 (Audience)", "users", "/Segments", [
                new("va-nav-40", "세그먼트 (Segments)", "resources", "/Segments"),
                new("va-nav-41", "잠재 고객 (Leads)", "users", "/Leads"),
                new("va-nav-42", "연락처 (Contacts)", "resources", "/Contacts")
            ]),
            new("va-nav-43", "마케팅 실행 (Execution)", "monitoring", "/CustomerJourneys", [
                new("va-nav-44", "고객 여정 (Customer Journeys)", "users", "/CustomerJourneys"),
                new("va-nav-45", "이메일 (Emails)", "resources", "/Emails"),
                new("va-nav-46", "마케팅 페이지 (Marketing Pages)", "monitoring", "/MarketingPages"),
                new("va-nav-47", "양식 (Forms)", "resources", "/MarketingForms"),
                new("va-nav-48", "이벤트 (Events)", "resources", "/Events")
            ]),
            new("va-nav-49", "참고자료 (Reference)", "resources", "/MarketingLists", [
                new("va-nav-50", "마케팅 목록 (Marketing Lists)", "monitoring", "/MarketingLists"),
                new("va-nav-51", "이메일 템플릿 (Email Templates)", "resources", "/EmailTemplates"),
                new("va-nav-52", "콘텐츠 블록 (Content Blocks)", "resources", "/ContentBlocks")
            ])
        ], Group: "VISUALACADEMY · BUSINESS"),
        new("va-nav-53", "고객 서비스 (Customer Service)", "users", "/Accounts", [
            new("va-nav-54", "고객 (Customers)", "users", "/Accounts", [
                new("va-nav-55", "고객 (Accounts)", "users", "/Accounts"),
                new("va-nav-56", "연락처 (Contacts)", "resources", "/Contacts")
            ]),
            new("va-nav-57", "서비스 (Service)", "resources", "/Cases", [
                new("va-nav-58", "사례 (Cases)", "resources", "/Cases"),
                new("va-nav-59", "대기열 (Queues)", "resources", "/Queues"),
                new("va-nav-60", "서비스 일정 (Service Schedule)", "resources", "/ScheduleBoard")
            ]),
            new("va-nav-61", "참고자료 (Reference)", "resources", "/KnowledgeBase", [
                new("va-nav-62", "지식 베이스 (Knowledge Base)", "resources", "/KnowledgeBase"),
                new("va-nav-63", "SLA (Service Level Agreements)", "resources", "/ServiceLevelAgreements")
            ])
        ], Group: "VISUALACADEMY · OPERATIONS"),
        new("va-nav-64", "현장 서비스 (Field Service)", "app-service", "/Assets", [
            new("va-nav-65", "자산/고객 (Assets/Customers)", "users", "/Assets", [
                new("va-nav-66", "자산 (Assets)", "resources", "/Assets"),
                new("va-nav-67", "고객 (Accounts)", "users", "/Accounts"),
                new("va-nav-68", "연락처 (Contacts)", "resources", "/Contacts")
            ]),
            new("va-nav-69", "운영 (Operations)", "resources", "/WorkOrders", [
                new("va-nav-70", "작업 주문 (Work Orders)", "resources", "/WorkOrders"),
                new("va-nav-71", "리소스 일정 (Resource Scheduling)", "resources", "/ResourceScheduling"),
                new("va-nav-72", "서비스 지역 (Service Territories)", "resources", "/ServiceTerritories")
            ]),
            new("va-nav-73", "참고자료 (Reference)", "resources", "/ServiceTasks", [
                new("va-nav-74", "서비스 작업 (Service Tasks)", "resources", "/ServiceTasks"),
                new("va-nav-75", "인시던트 유형 (Incident Types)", "resources", "/IncidentTypes"),
                new("va-nav-76", "가격표 (Price Lists)", "resources", "/PriceLists")
            ])
        ], Group: "VISUALACADEMY · OPERATIONS"),
        new("va-nav-77", "프로젝트 서비스 (Project Operations)", "resources", "/Projects", [
            new("va-nav-78", "프로젝트 (Projects)", "resources", "/Projects", [
                new("va-nav-79", "프로젝트 (Projects)", "resources", "/Projects"),
                new("va-nav-80", "작업 (Tasks)", "resources", "/Tasks")
            ]),
            new("va-nav-81", "실행 (Delivery)", "resources", "/TimeEntries", [
                new("va-nav-82", "시간 입력 (Time Entries)", "resources", "/TimeEntries"),
                new("va-nav-83", "비용 입력 (Expense Entries)", "resources", "/ExpenseEntries")
            ]),
            new("va-nav-84", "참고자료 (Reference)", "resources", "/Contracts", [
                new("va-nav-85", "계약 (Contracts)", "resources", "/Contracts"),
                new("va-nav-86", "가격표 (Price Lists)", "resources", "/PriceLists")
            ])
        ], Group: "VISUALACADEMY · OPERATIONS")
    ];

    public static DashboardNavigationViewModel Build(string? currentPath)
    {
        var normalized = Normalize(currentPath);
        return new DashboardNavigationViewModel
        {
            Items = Navigation.Select(x => BuildNode(x, normalized, 0)).ToArray()
        };
    }

    private static DashboardNavigationNodeViewModel BuildNode(DashboardNavigationItem item, string currentPath, int depth)
    {
        var children = (item.Children ?? [])
            .Select(x => BuildNode(x, currentPath, depth + 1)).ToArray();
        var isCurrent = string.Equals(Normalize(item.Href), currentPath, StringComparison.OrdinalIgnoreCase);
        var containsCurrent = isCurrent || children.Any(x => x.ContainsCurrent);
        return new DashboardNavigationNodeViewModel
        {
            Key = item.Key, Text = item.Text, Icon = item.Icon, Href = item.Href,
            Depth = depth, IsCurrent = isCurrent, ContainsCurrent = containsCurrent,
            IsExpanded = item.ExpandedByDefault || containsCurrent,
            Group = item.Group, Children = children
        };
    }

    private static string Normalize(string? path)
    {
        var normalized = string.IsNullOrWhiteSpace(path) ? "/" : path.Trim();
        return normalized.Length > 1 ? normalized.TrimEnd('/').ToLowerInvariant() : normalized.ToLowerInvariant();
    }
}
