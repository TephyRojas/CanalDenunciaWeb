using CanalDenuncia.Data.Package.UsuarioRolPkg;
using CanalDenunciaWeb.Data.Package.DepartamentoPkg;
using CanalDenunciaWeb.Data.Package.DepartamentoSedePkg;
using CanalDenunciaWeb.Data.Package.EstadoPkg;
using CanalDenunciaWeb.Data.Package.SedePkg;
using CanalDenunciaWeb.Data.Package.TipoDelitoPkg;
using CanalDenunciaWeb.Data.Package.UsuarioRolPkg;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CanalDenunciaWeb.Helper
{
    public class Combobox
    {
        SedeService sedeService = new SedeService();
        DepartamentoService departamentoService = new DepartamentoService();
        TipoDelitoService tipoDelitoService = new TipoDelitoService();
        EstadoService estadoService = new EstadoService();
        DepartamentoSedeService departamentoSedeService = new DepartamentoSedeService();
        UsuarioRolService rolService = new UsuarioRolService();

        Util util = new Util();
        public List<SedeComboboxOutputDto> ComboboxSede(string token, int opcionSeleccione = 1)
        {
            List<SedeComboboxOutputDto> sedeComboboxOutputDtos = new List<SedeComboboxOutputDto>();

            var resultSedeCombobox = sedeService.SelectListCombobox(util.UrlServicios, token);


            if (resultSedeCombobox.Respuesta.CodigoRetorno == 1)
            {
                if (resultSedeCombobox.Resultado.Data.Count > 0)
                {
                    foreach (var item in resultSedeCombobox.Resultado.Data)
                    {
                        var result = JsonConvert.DeserializeObject<SedeComboboxOutputDto>(item.ToString());
                        sedeComboboxOutputDtos.Add(result);
                    }
                }
            }
            sedeComboboxOutputDtos = sedeComboboxOutputDtos.Where(x => x.IdSede != 0).ToList();
            if (opcionSeleccione == 1)
            {
                SedeComboboxOutputDto seleccione = new SedeComboboxOutputDto()
                {
                    IdSede = 0,
                    Descripcion = " - Seleccione - "
                };

                sedeComboboxOutputDtos.Add(seleccione);
            }

            sedeComboboxOutputDtos = sedeComboboxOutputDtos.OrderBy(x => x.Descripcion).ToList();
            return sedeComboboxOutputDtos;

        }

        public List<DepartamentoComboboxOutputDto> ComboboxDepartamento(string token, int opcionSeleccione = 1, int idSede = 1)
        {
            List<DepartamentoComboboxOutputDto> departamentoComboboxOutputDtos = new List<DepartamentoComboboxOutputDto>();

            var resultTipoDelitoCombobox = departamentoService.SelectListCombobox(util.UrlServicios, token, idSede);


            if (resultTipoDelitoCombobox.Respuesta.CodigoRetorno == 1)
            {
                if (resultTipoDelitoCombobox.Resultado.Data.Count > 0)
                {
                    foreach (var item in resultTipoDelitoCombobox.Resultado.Data)
                    {
                        var result = JsonConvert.DeserializeObject<DepartamentoComboboxOutputDto>(item.ToString());
                        departamentoComboboxOutputDtos.Add(result);
                    }
                }
            }
            departamentoComboboxOutputDtos = departamentoComboboxOutputDtos.Where(x => x.IdDepartamento != 0).ToList();
            if (opcionSeleccione == 1)
            {
                DepartamentoComboboxOutputDto seleccione = new DepartamentoComboboxOutputDto()
                {
                    IdDepartamento = 0,
                    Nombre = " - Seleccione - "
                };

                departamentoComboboxOutputDtos.Add(seleccione);
            }

            departamentoComboboxOutputDtos = departamentoComboboxOutputDtos.OrderBy(x => x.Nombre).ToList();
            return departamentoComboboxOutputDtos;

        }

        public List<DepartamentoSedeComboboxOutputDto> ComboboxDepartamentoSede(string token, int opcionSeleccione = 1, int idSede = 1)
        {
            List<DepartamentoSedeComboboxOutputDto> departamentoComboboxOutputDtos = new List<DepartamentoSedeComboboxOutputDto>();

            var resultTipoDelitoCombobox = departamentoSedeService.SelectListCombobox(util.UrlServicios, token, idSede.ToString());


            if (resultTipoDelitoCombobox.Respuesta.CodigoRetorno == 1)
            {
                if (resultTipoDelitoCombobox.Resultado.Data.Count > 0)
                {
                    foreach (var item in resultTipoDelitoCombobox.Resultado.Data)
                    {
                        var result = JsonConvert.DeserializeObject<DepartamentoSedeComboboxOutputDto>(item.ToString());
                        departamentoComboboxOutputDtos.Add(result);
                    }
                }
            }
            departamentoComboboxOutputDtos = departamentoComboboxOutputDtos.Where(x => x.IdDepartamentoSede != 0).ToList();
            if (opcionSeleccione == 1)
            {
                DepartamentoSedeComboboxOutputDto seleccione = new DepartamentoSedeComboboxOutputDto()
                {
                    IdDepartamentoSede = 0,
                    Nombre = " - Seleccione - "
                };

                departamentoComboboxOutputDtos.Add(seleccione);
            }

            departamentoComboboxOutputDtos = departamentoComboboxOutputDtos.OrderBy(x => x.Nombre).ToList();
            return departamentoComboboxOutputDtos;

        }
        public List<TipoDelitoComboboxOutputDto> ComboboxTipoDelito(string token, int opcionSeleccione = 1)
        {
            List<TipoDelitoComboboxOutputDto> tipoDelitoComboboxOutputDtos = new List<TipoDelitoComboboxOutputDto>();

            var resultTipoDelitoCombobox = tipoDelitoService.SelectListCombobox(util.UrlServicios, token);


            if (resultTipoDelitoCombobox.Respuesta.CodigoRetorno == 1)
            {
                if (resultTipoDelitoCombobox.Resultado.Data.Count > 0)
                {
                    foreach (var item in resultTipoDelitoCombobox.Resultado.Data)
                    {
                        var result = JsonConvert.DeserializeObject<TipoDelitoComboboxOutputDto>(item.ToString());
                        tipoDelitoComboboxOutputDtos.Add(result);
                    }
                }
            }
            tipoDelitoComboboxOutputDtos = tipoDelitoComboboxOutputDtos.Where(x => x.IdTipoDelito != 0).ToList();
            if (opcionSeleccione == 1)
            {
                TipoDelitoComboboxOutputDto seleccione = new TipoDelitoComboboxOutputDto()
                {
                    IdTipoDelito = 0,
                    Nombre = " - Seleccione - "
                };

                tipoDelitoComboboxOutputDtos.Add(seleccione);
            }

            tipoDelitoComboboxOutputDtos = tipoDelitoComboboxOutputDtos.OrderBy(x => x.Nombre).ToList();
            return tipoDelitoComboboxOutputDtos;

        }
        public List<EstadoComboboxOutputDto> ComboboxEstado(string token, int opcionSeleccione = 1)
        {
            List<EstadoComboboxOutputDto> estadoComboboxOutputDtos = new List<EstadoComboboxOutputDto>();

            var resultEstadoCombobox = estadoService.SelectListCombobox(util.UrlServicios, token);


            if (resultEstadoCombobox.Respuesta.CodigoRetorno == 1)
            {
                if (resultEstadoCombobox.Resultado.Data.Count > 0)
                {
                    foreach (var item in resultEstadoCombobox.Resultado.Data)
                    {
                        var result = JsonConvert.DeserializeObject<EstadoComboboxOutputDto>(item.ToString());
                        estadoComboboxOutputDtos.Add(result);
                    }
                }
            }
            estadoComboboxOutputDtos = estadoComboboxOutputDtos.Where(x => x.IdEstadoDenuncia != 0).ToList();
            if (opcionSeleccione == 1)
            {
                EstadoComboboxOutputDto seleccione = new EstadoComboboxOutputDto()
                {
                    IdEstadoDenuncia = 0,
                    Nombre = " - Seleccione - "
                };

                estadoComboboxOutputDtos.Add(seleccione);
            }

            estadoComboboxOutputDtos = estadoComboboxOutputDtos.OrderBy(x => x.Nombre).ToList();
            return estadoComboboxOutputDtos;

        }

        public List<RolComboboxOutputDto> ComboboxRol(string token, string idUsuario, int opcionSeleccione = 1)
        {
            List<RolComboboxOutputDto> rolComboboxOutputDtos = new List<RolComboboxOutputDto>();

            var resultRolCombobox = rolService.SelectListCombobox(util.UrlServicios, token,idUsuario);


            if (resultRolCombobox.Respuesta.CodigoRetorno == 1)
            {
                if (resultRolCombobox.Resultado.Data.Count > 0)
                {
                    foreach (var item in resultRolCombobox.Resultado.Data)
                    {
                        var result = JsonConvert.DeserializeObject<RolComboboxOutputDto>(item.ToString());
                        rolComboboxOutputDtos.Add(result);
                    }
                }
            }
            rolComboboxOutputDtos = rolComboboxOutputDtos.Where(x => x.IdRol != 0).ToList();
            if (opcionSeleccione == 1)
            {
                RolComboboxOutputDto seleccione = new RolComboboxOutputDto()
                {
                    IdRol = 0,
                    Nombre = " - Seleccione - "
                };

                rolComboboxOutputDtos.Add(seleccione);
            }

            rolComboboxOutputDtos = rolComboboxOutputDtos.OrderBy(x => x.Nombre).ToList();
            return rolComboboxOutputDtos;

        }
    }
}