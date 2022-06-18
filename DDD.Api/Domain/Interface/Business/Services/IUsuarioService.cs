using DDD.Api.Domain.Models.BusnModel;

namespace DDD.Api.Domain.Interface.Business.Services;
public interface IUsuarioService
{
    Task<int> GetIdUsuarioImportarExportarAsync();
    Task<int> GetIdUsuarioAdminRootAsync();
    bool PodeVisualizarUsuario(UsuarioBusnModel usuario, int idUsuarioImportarExportar);
    bool PodeCadastrarUsuario(UsuarioBusnModel usuario, int idUsuarioImportarExportar, int idUsuarioAdminRoot);
    bool PodeExcluirUsuario(UsuarioBusnModel usuario, int idUsuarioImportarExportar, int idUsuarioAdminRoot);
    Task<List<UsuarioBusnModel>> ListarTodosAsync();
    Task<UsuarioBusnModel> GetByIdAsync(int id);
    Task<UsuarioBusnModel> CreateAsync(UsuarioBusnModel usuario);
    Task<UsuarioBusnModel> UpdateAsync(UsuarioBusnModel usuario);
    Task RemoveAsync(int id);
    Task CarregarUsuariosAdminRootEImportarExportarAsync();
}