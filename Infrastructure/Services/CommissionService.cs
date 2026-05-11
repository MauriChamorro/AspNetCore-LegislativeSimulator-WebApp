using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Infrastructure.Services;

public class CommissionService : ICommissionService
{
    private readonly IReferralCommissionRepository _referralCommissionRepository;

    private List<Commission> _commissions; //this is a inmemory repo for now

    public CommissionService(IReferralCommissionRepository referralCommissionRepository)
    {
        _referralCommissionRepository = referralCommissionRepository;
        _commissions = new List<Commission>
        {
            new()
            {
                CommissionId = 1,
                Name = "Comisión de Asuntos Constitucionales",
                WordsForAssingment =
                [
                    "Constitución", "Reforma", "Electoral", "Intervención", "Privilegios", "Tratados", "Ciudadanía",
                    "Federalismo", "Poderes", "Enmienda", "Protocolo", "Institucional", "Representación", "Sufragio",
                    "Autonomía", "Competencia", "Tratado", "Decretos", "Reglamentación", "Ética"
                ]
            },
            new()
            {
                CommissionId = 2,
                Name = "Comisión de Presupuesto y Hacienda",
                WordsForAssingment =
                [
                    "Gasto", "Tributo", "Impuesto", "Alícuota", "Financiamiento", "Crédito", "Deuda", "Coparticipación",
                    "Déficit", "Inversión", "Partida", "Erario", "Fiscal", "Recaudación", "Bonos", "Aranceles",
                    "Exención", "Devengado", "Tesoro", "Superávit"
                ]
            },
            new()
            {
                CommissionId = 3,
                Name = "Comisión de Legislación General",
                WordsForAssingment =
                [
                    "Código", "Contrato", "Civil", "Propiedad", "Locación", "Sucesiones", "Personería", "Notarial",
                    "Registro", "Alquiler", "Sociedades", "Normativa", "Capacidad", "Patrimonio", "Domicilio",
                    "Arrendamiento", "Prescripción", "Obligaciones", "Comercial", "Fundaciones"
                ]
            },
            new()
            {
                CommissionId = 4,
                Name = "Comisión de Legislación del Trabajo",
                WordsForAssingment =
                [
                    "Empleo", "Indemnización", "Gremio", "Sindicato", "Salario", "Jornada", "Patronal", "Cese",
                    "Previsión", "Jubilación", "Paritaria", "Convenio", "Aporte", "Contribución", "Despido", "ART",
                    "Riesgo", "Licencia", "Seguridad", "Obrero"
                ]
            },
            new()
            {
                CommissionId = 5,
                Name = "Comisión de Acción Social y Salúd Pública",
                WordsForAssingment =
                [
                    "Sanitario", "Epidemiología", "Prevención", "Paciente", "Médico", "Farmacéutico", "Adicciones",
                    "Hospital", "Clínica", "Tratamiento", "Discapacidad", "Infancia", "Vulnerabilidad",
                    "Medicamento", "Bioética", "Asistencia", "Vacunación", "Mental", "Nutrición", "Prestación"
                ]
            },
            new()
            {
                CommissionId = 6,
                Name = "Comisión de Energía y Combustible",
                WordsForAssingment =
                [
                    "Hidrocarburos", "Petróleo", "Gas", "Renovables", "Tarifas", "Eléctrica", "Minería", "Litio",
                    "Sustentable", "Generación", "Transporte", "Distribución", "Regalías", "Refinería",
                    "Biocombustible", "Eólica", "Solar", "Cuenca", "Reservas", "Yacimiento"
                ]
            },
            new()
            {
                CommissionId = 7,
                Name = "Comisión de Juicio Político",
                WordsForAssingment =
                [
                    "Destitución", "Mal desempeño", "Acusación", "Denuncia", "Remoción", "Investigación", "Magistrado",
                    "Funcionario", "Corte", "Proceso", "Causal", "Testimonio", "Probatorio", "Dictamen", "Fallo",
                    "Inhabilidad", "Debido proceso", "Senado", "Cargo", "Defensa"
                ]
            }
        };
    }

    public List<Commission> EvaluateCommissionFor(string projectArticles)
    {
        var assignedCommissions = new List<Commission>();
        var articles = projectArticles.ToLower();
        foreach (var commission in _commissions)
        {
            foreach (var commissionWord in commission.WordsForAssingment)
            {
                if (articles.Contains(commissionWord, StringComparison.OrdinalIgnoreCase))
                {
                    assignedCommissions.Add(commission);
                    break;
                }
            }
        }

        return assignedCommissions;
    }

    public List<ReferralCommission> AssignCommissionTo(List<Commission> commissions, int projectId)
    {
        var referralCommissions = new List<ReferralCommission>();
        foreach (var commission in commissions)
        {
            referralCommissions.Add(
                new ReferralCommission
                {
                    ProjectId = projectId,
                    CommissionId = commission.CommissionId,
                    CommisionName = commission.Name,
                    State = ReferralCommissionState.Assigned,
                    ReferralDate = DateTime.Now
                }
            );
        }

        _referralCommissionRepository.AddRange(referralCommissions);
        return referralCommissions;
    }

    public bool HasBeenAssigned(int projectId) =>
        _referralCommissionRepository.ExistProjectId(projectId);

    public List<ReferralCommission> GetReferralCommissionsFor(int projectId) => 
        _referralCommissionRepository.GetFor(projectId);

    public ReferralCommission GetActualReferral(List<ReferralCommission> referralCommissions)
    {
        if (referralCommissions.Any(rc => rc.State == ReferralCommissionState.Evaluating))
            return referralCommissions.First(rc => rc.State == ReferralCommissionState.Evaluating);
        return referralCommissions.First(rc => rc.State == ReferralCommissionState.Assigned);
    }

    public void DoNextReferralPhase(ReferralCommission actualReferral)
    {
        if (actualReferral.State == ReferralCommissionState.Assigned)
        {
            actualReferral.State = ReferralCommissionState.Evaluating;
            actualReferral.ReferralDate = DateTime.Now;
        }
        else if (actualReferral.State == ReferralCommissionState.Evaluating)
        {
            actualReferral.State = GetRandomResult();
            actualReferral.ReferralDate = DateTime.Now;
        }
    }

    private static ReferralCommissionState GetRandomResult()
    {
        var rnd = new Random();
        var success = rnd.Next(2) == 0;
        if (success)
            return ReferralCommissionState.Accepted;
        return ReferralCommissionState.Rejected;
    }

    public bool ThereAreNotPendingReferral(List<ReferralCommission> referralCommissions) =>
        referralCommissions.TrueForAll(rc =>
            rc.State == ReferralCommissionState.Accepted || rc.State == ReferralCommissionState.Rejected);

    public bool ReferralIsRejected(ReferralCommission actualReferral) =>
        actualReferral.State == ReferralCommissionState.Rejected;

    public bool AcceptedByAllCommission(int projectId) =>
        _referralCommissionRepository.GetFor(projectId)
            .TrueForAll(rc => rc.State == ReferralCommissionState.Accepted);
}