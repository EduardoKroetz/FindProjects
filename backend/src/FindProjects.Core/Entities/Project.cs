using FindProjects.Core.Common;
using FindProjects.Core.Entities.Base;
using FindProjects.Core.Enums;

namespace FindProjects.Core.Entities;

public class Project : Entity
{
    internal Project()
    {
        
    }
    
    public Project(string title, string description, User user, DateTime? deadLine, string? projectLink, bool hasBudget, decimal? budget ,int maxContributors)
    {
        UpdateTitle(title);
        UpdateDescription(description);
        UpdateDeadLine(deadLine);
        UpdateHasBudget(hasBudget);
        UpdateBudget(budget);
        UpdateMaxContributors(maxContributors);
        UpdateProjectLink(projectLink);
        UpdateStatus(EProjectStatus.Active);
        UserId = user.Id;
        User = user;
        CreatedAt = DateTime.UtcNow;
    }

    public string Title { get; private set;  }
    public string Description { get; private set;  }
    public string UserId { get; private set;  }
    public User? User { get; private set;  }
    public EProjectStatus Status { get; private set;  }
    public DateTime? DeadLine { get; private set;  }
    public bool HasBudget { get; private set;  }
    public decimal? Budget { get; private set;  }
    public int MaxContributors { get; private set;  } 
    public DateTime CreatedAt { get; private set;  }
    public string? ProjectLink { get; private set;  }
    
    public ICollection<Skill> Skills { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Contributor> Contributors { get; set; } = [];
    public ICollection<ProjectMessage> ProjectMessages { get; set; } = [];
    
    //Setters
    public Result Update(string title, string description, DateTime? deadLine, string? projectLink, bool hasBudget, decimal? budget, int maxContributors)
    {
        var results = new List<Result>()
        {
            UpdateTitle(title),
            UpdateDescription(description),
            UpdateDeadLine(deadLine),
            UpdateHasBudget(hasBudget),
            UpdateBudget(budget),
            UpdateMaxContributors(maxContributors),
            UpdateProjectLink(projectLink),
        };

        foreach (var result in results )
        {
            if (result.IsSuccess == false)
                return result;
        }

        return Result.Success();
    }
    
    public Result UpdateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            return Result.Failed("Título inválido");

        if (title.Length > 100)
            return Result.Failed("Título deve possuir no máximo 100 caracteres");

        Title = title;
        return Result.Success();
    }
    
    public Result UpdateDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            return Result.Failed("Descrição inválida");

        if (description.Length > 3000)
            return Result.Failed("Descrição deve possuir no máximo 3000 caracteres");

        Description = description;
        return Result.Success();
    }
    
    public Result UpdateDeadLine(DateTime? deadLine)
    {
        if (deadLine != null && deadLine < DateTime.UtcNow)
            return Result.Failed("O prazo final do projeto deve ser maior que a data atual");

        DeadLine = deadLine;
        return Result.Success();
    }
    
    public Result UpdateHasBudget(bool hasBudget)
    {
        if (hasBudget == false)
            Budget = null;

        HasBudget = hasBudget;
        return Result.Success();
    }
    
    public Result UpdateBudget(decimal? budget)
    {
        if (HasBudget == true && budget <= 0)
            return Result.Failed("O orçamento do projeto deve ser maior que 0");
        
        Budget = budget;
        if (HasBudget == false)
            Budget = null;
        
        return Result.Success();
    }
    
    public Result UpdateMaxContributors(int maxContributors)
    {
        if (maxContributors <= 0)
            return Result.Failed("O máximo de contribuidores do projeto deve ser maior que 0");

        MaxContributors = maxContributors;
        return Result.Success();
    }
    
    public Result UpdateProjectLink(string? projectLink)
    {
        ProjectLink = projectLink;
        return Result.Success();
    }

    public Result UpdateStatus(EProjectStatus status)
    {
        Status = status;
        return Result.Success();
    }
}