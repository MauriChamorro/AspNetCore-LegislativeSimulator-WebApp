using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;

namespace WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;

public class InMemoryProjectRepository : IProjectRepository
{
    private readonly List<Project> _projects;

    public InMemoryProjectRepository()
    {
        _projects = new List<Project>();
        _projects.Add(
            new Project
            {
                ProjectId = 1,
                Title = "Ley de Bases y Puntos de Partida para la Libertad de los Argentinos",
                Articles = "Art. 1°.- Declárase la emergencia pública en materia administrativa, económica, financiera y energética por el plazo de un (1) año.\n\nArt. 7°.- Decláranse \"sujetas a privatización\", en los términos y con los alcances de la Ley N° 23.696, las empresas y sociedades de propiedad total o parcial del Estado nacional enumeradas en el Anexo I.\n\nArt. 31°.- El personal de la Administración Pública Nacional que se encuentre afectado por medidas de reestructuración que comporten la supresión de organismos o dependencias, quedará en situación de disponibilidad por un periodo máximo de doce (12) meses.",
                Fundaments = "La presente ley busca revertir la crisis terminal que atraviesa la Nación, eliminando el peso del Estado sobre la actividad privada y devolviendo la libertad de decisión a los ciudadanos.",
                Summary = "Marco legal que otorga facultades delegadas al Ejecutivo para reformar el Estado, privatizar empresas y flexibilizar la estructura administrativa nacional.",
                StateHistory = new ProjectStateHistory
                {
                    ProjectState = new ProjectState
                    {
                        State = FileState.Scratch
                    },
                    Date = DateTime.Now.Subtract(TimeSpan.FromDays(1))
                }
            });
        _projects.Add(
            new Project
            {
                ProjectId = 2,
                Title = "Régimen de Incentivo para Grandes Inversiones (RIGI)",
                Articles = "Art. 165°.- Créase el RIGI, bajo cuya órbita podrán acogerse los \"Vehículos de Proyecto Único\" (VPU) que realicen inversiones superiores a los DÓLARES ESTADOUNIDENSES DOSCIENTOS MILLONES (USD 200.000.000).\n\nArt. 182°.- La alícuota del Impuesto a las Ganancias para los VPU adheridos al RIGI será del VEINTICINCO POR CIENTO (25%), no resultando de aplicación las alícuotas escalonadas previstas en la ley general.\n\nArt. 196°.- Las exportaciones de productos resultantes de los proyectos adheridos estarán exentas de derechos de exportación transcurridos tres (3) años desde la fecha de adhesión.",
                Fundaments = "La falta de inversión de capital intensivo requiere de un marco que garantice estabilidad jurídica y fiscal por un plazo extendido para atraer proyectos de escala global.",
                Summary = "Un régimen especial que ofrece beneficios fiscales, aduaneros y cambiarios por 30 años a empresas que realicen inversiones masivas en sectores estratégicos.",
                StateHistory = new ProjectStateHistory
                {
                    ProjectState = new ProjectState
                    {
                        State =  FileState.PendingForAssignCommissions
                    },
                    Date = DateTime.Now.Subtract(TimeSpan.FromDays(5))
                }
            });
        _projects.Add(
            new Project
            {
                ProjectId = 3,
                Title = "Actualización de la Ley de Contrato de Trabajo",
                Articles = "Art. 92 bis.- El contrato de trabajo de tiempo indeterminado se entenderá celebrado a prueba durante los primeros seis (6) meses de vigencia. Las convenciones colectivas podrán ampliar este plazo hasta ocho (8) o doce (12) meses según el tamaño de la empresa.\n\nArt. 81°.- Los trabajadores independientes podrán contar con hasta otros tres (3) trabajadores independientes para llevar adelante un emprendimiento productivo, bajo un régimen simplificado sin relación de dependencia.\n\nArt. 245 bis.- Las partes podrán sustituir el régimen indemnizatorio por un fondo o sistema de cese laboral cuyo costo estará siempre a cargo del empleador, según se establezca en el Convenio Colectivo de Trabajo.",
                Fundaments = "Necesidad de adaptar la normativa laboral a las nuevas formas de producción y reducir la litigiosidad que afecta la creación de empleo genuino en el sector privado.",
                Summary = "Reforma que flexibiliza el período de prueba, permite esquemas de colaboradores autónomos y habilita la creación de fondos de cese laboral alternativos a la indemnización tradicional.",
                StateHistory = new ProjectStateHistory
                {
                    ProjectState = new ProjectState
                    {
                        State =  FileState.Scratch
                    },
                    Date = DateTime.Now
                }
            });
        _projects.Add(
            new Project
            {
                ProjectId = 4,
                Title = "Modificación al Código Civil y Comercial - Locaciones",
                Articles = "Art. 1198°.- El plazo de las locaciones con cualquier destino será el que las partes hayan establecido. En caso de que no lo establezcan, será de dos (2) años para locación habitacional.\n\nArt. 1199°.- Los alquileres podrán establecerse en moneda de curso legal o en moneda extranjera, al libre arbitrio de las partes. El locatario no podrá exigir que se acepte el pago en una moneda distinta a la pactada.\n\nArt. 1201°.- Las partes podrán pactar el ajuste del valor de los alquileres utilizando cualquier índice, público o privado, expresado en la misma moneda en la que se pactó la locación.",
                Fundaments = "El exceso de regulación estatal provocó la desaparición de la oferta de viviendas. La autonomía de la voluntad permite sincerar los precios y aumentar la disponibilidad de inmuebles.",
                Summary = "Desregulación total de los contratos de alquiler, permitiendo acuerdos libres en plazos, moneda (pesos o dólares) e índices de actualización.",
                StateHistory = new ProjectStateHistory
                {
                    ProjectState = new ProjectState
                    {
                        State =  FileState.ApprovedInSession
                    },
                    Date = DateTime.Now.Subtract(TimeSpan.FromDays(15))
                }
            });
        _projects.Add(
            new Project
            {
                ProjectId = 5,
                Title = "Ley de Medidas Fiscales Paliativas y Relevantes",
                Articles = "Art. 38°.- Establécese un Régimen de Regularización de Activos para sujetos residentes y no residentes, permitiendo la declaración de fondos sin el pago de impuestos por los primeros USD 100.000.\n\nArt. 73°.- Sustitúyese el impuesto a las Ganancias de la cuarta categoría por el \"Impuesto a los Ingresos Personales\", estableciendo nuevas escalas y deducciones que se actualizarán semestralmente por IPC.\n\nArt. 101°.- Modifícanse los importes del Régimen Simplificado para Pequeños Contribuyentes (Monotributo), incrementando los topes de facturación anual para todas las categorías.",
                Fundaments = "Es imperativo mejorar la recaudación fiscal mediante tributos más equitativos y fomentar el ingreso de capitales al sistema formal mediante un blanqueo accesible.",
                Summary = "Reforma impositiva que reintroduce el impuesto a los altos ingresos personales, actualiza el monotributo y crea un régimen de blanqueo de capitales.",
                StateHistory = new ProjectStateHistory
                {
                    ProjectState = new ProjectState
                    {
                        State =  FileState.RejectedInSession
                    },
                    Date = DateTime.Now.Subtract(TimeSpan.FromDays(3))
                }
            });
    }

    public List<Project> GetProjects() => 
        _projects.ToList();

    public Project GetProjectById(int id) => 
        _projects.First(p => p.ProjectId == id);

    public void Add(Project project) => _projects.Add(project);

    public int GetLastId() => _projects.Last().ProjectId;
    
    public void Delete(int projectId) => 
        _projects.Remove(_projects.First(p => p.ProjectId == projectId));
}