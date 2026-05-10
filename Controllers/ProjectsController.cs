using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Services;
using WebAppMVC.Filters.ActionFilters;
using WebAppMVC.Filters.ExceptionFilters;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

[UnexpectedHandleException]
public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IProjectViewModelService _projectViewModelService;
    private readonly ICommissionService _commissionService;

    public ProjectsController(IProjectService projectService,
        IProjectViewModelService projectViewModelService,
        ICommissionService commissionService)
    {
        _projectService = projectService;
        _projectViewModelService = projectViewModelService;
        _commissionService = commissionService;
    }

    public IActionResult Index()
    {
        var projects = _projectService.GetProjects();
        var projectVms = _projectViewModelService.ToProjectsVm(projects);
        return View(projectVms);
    }

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Action = "add";
        var emptyProject = _projectService.CreatEmptyProject();
        var newProjectVm = _projectViewModelService.ToProjectVm(emptyProject);
        return View(newProjectVm);
    }
    
    [HttpPost]
    public IActionResult Add(ProjectViewModel projectVm)
    {
        ViewBag.Action = "add";
        
        //TODO: check if only title is required for this phase
        if (!ModelState.IsValid)
        {
            var emptyProject = _projectService.CreatEmptyProject();
            _projectViewModelService.UpdateMissingValues(projectVm, emptyProject);
            return View(projectVm);
        }

        _projectService.CreateNewProject(projectVm.Title, projectVm.Articles, projectVm.Fundaments,projectVm.Summary);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet("Projects/Edit/{projectId}")]
    [ProjectIdNotFoundFilter]
    public IActionResult Edit([FromRoute]int projectId)
    {
        ViewBag.Action = "edit";
        
        var project = _projectService.GetProjectById(projectId);
        
        var projectVm = _projectViewModelService.ToProjectVm(project);
        if (_commissionService.HasBeenAssigned(projectId))
        {
            var referralCommissions = _commissionService.GetReferralCommissionsFor(projectId);
            _projectViewModelService.SetVmCommissions(projectVm, referralCommissions);
        }
        return View(projectVm);
    }

    [HttpPost]
    public IActionResult Edit(ProjectViewModel projectVm)
    {
        ViewBag.Action = "edit";
        
        //TODO: id exist validation
        //TODO: Validation: cannot edit in different to scratch
        if (!_projectService.ExistProject(projectVm.ProjectId))
            return BadRequest("El proyecto no existe");
        
        var savedProject = _projectService.GetProjectById(projectVm.ProjectId);
        
        if (!ModelState.IsValid)
        {
            //re fill fields
            _projectViewModelService.UpdateMissingValues(projectVm, savedProject);
            return View(projectVm);
        }

        _projectService.EditProject(
            savedProject,
            projectVm.Title, 
            projectVm.Articles,
            projectVm.Fundaments,
            projectVm.Summary);
        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult SendToCommission(ProjectViewModel projectVm)
    {
        //validations
        //no es error de usuario, es preventivo de api
        if (!_projectService.ExistProject(projectVm.ProjectId))
            return BadRequest("El proyecto no existe");
        
        var savedProject = _projectService.GetProjectById(projectVm.ProjectId);
        
        //validar que todos los campos estén llenos
        //en el mundo real, habrían más validaciones
        if (!ModelState.IsValid)
        {
            _projectViewModelService.UpdateMissingValues(projectVm, savedProject);
            ViewBag.Action = "edit";
            return View("Edit", projectVm); //doesnt clear data for on back validation
        }

        if(!_projectService.CanSendToCommission(savedProject))
            return BadRequest("No es posible enviar a comisión");

        _projectService.SendToCommissions(savedProject);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public IActionResult Delete(int projectId)
    {
        if (!_projectService.ExistProject(projectId))
            return View("Error");

        if (!_projectService.CanDelete(projectId))
            return View("Error");

        _projectService.DeleteProject(projectId);
        return RedirectToAction(nameof(Index));
    }
}