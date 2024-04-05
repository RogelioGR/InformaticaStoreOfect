using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using ApplicationCore.Wrappers;
using AutoMapper;
using ApplicationCore.Commads;
using Dapper;
using Infraestructure.Persistence;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.DTOs.User;
using System.Net.Sockets;
using System.Net;
using ApplicationCore.Commands;
using Domain.Entities;
using Infraestructure.Tenant.Models;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;


namespace Infraestructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public DashboardService(ApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Response<object>> GetData()
        {
            object list = new object();
            list = await _dbContext.Usuarios.ToListAsync();
            return new Response<object>(list);
        }

        public async Task<Response<int>> Create(UserTDO request)
        {
            var newPersona = _mapper.Map<Domain.Entities.Usuarios>(request);
            await _dbContext.Usuarios.AddAsync(newPersona);
            await _dbContext.SaveChangesAsync();
            return new Response<int>(1);

        }
        public async Task<Response<int>> Update(int id, UserTDO request)
        {
            var persona = await _dbContext.Usuarios.FindAsync(id);
            if (persona == null)
            {
                return new Response<int>(0);
            }

            _mapper.Map(request, persona);

            _dbContext.Usuarios.Update(persona);
            await _dbContext.SaveChangesAsync();

            return new Response<int>(1);
        }

        public async Task<Response<string>> GetClientIpAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            var ipAddressString = ipAddress?.ToString() ?? "Error en la direccion";
            return new Response<string>(ipAddressString);

        }

        public async Task<Response<int>> GetLogsCreate(logsDTO request)
        
       {

            var ip = GetClientIpAddress().Result.Message;
            var c = new CreateLogsCommand();
            c.ip = ip;
            c.nombrefuncion = request.nombrefuncion;
            c.Mensaje_ = request.Mensaje_;
            c.Fecha = request.Fecha;
            c.Datos = request.Datos;
            c.Status = request.Status;

            var ca = _mapper.Map<Domain.Entities.logs>(c);
            await _dbContext.logs.AddAsync(ca);
            await _dbContext.SaveChangesAsync();
            return new Response<int>(ca.id_logs, "Registro creado");
        }

        public async Task<Response<int>> Createpersona(UserTDO request)
        {
            try
            {
                var us = _mapper.Map<Usuarios>(request);
                await _dbContext.Usuarios.AddAsync(us);
                var req = await _dbContext.SaveChangesAsync();
                var res = new Response<int>(us.IDusuario, "Registro Creado");

                //logs
                var log = new logsDTO();
                //log.Datos = request.Nombre + request.Apellido + request.Edad + request.Correo + request.Numero;
                log.Datos = JsonConvert.SerializeObject(request);
                log.Fecha = DateTime.Now.ToString();
                log.ip = GetClientIpAddress().Result.Message;
                log.Status = "200";
                log.Mensaje_ = res.Message;
                log.nombrefuncion = "createusers";

                await GetLogsCreate(log);
                return res;
            }

            catch (Exception ex)
            {
                // Manejar otras excepciones
                var errorLog = new logsDTO();
                //errorLog.Datos = request.Nombre + request.Apellido + request.Edad + request.Correo + request.Numero;
                errorLog.Datos = JsonConvert.SerializeObject(request);
                errorLog.Fecha = DateTime.Now.ToString();
                errorLog.ip = GetClientIpAddress().Result.Message;
                errorLog.nombrefuncion = "CreateUser";

                if (ex.InnerException != null)
                {
                    errorLog.Mensaje_ = $"Error desconocido al crear el registro. Mensaje interno: {ex.InnerException.Message}";
                }
                else
                {
                    errorLog.Mensaje_ = "Error desconocido al crear el registro";
                }

                errorLog.Status = "500";

                await GetLogsCreate(errorLog);
                throw;
            }
        }

        public async Task<Response<int>> Updatepersona(int id, UserTDO request)
        {
            try
            {
                var existingUser = await _dbContext.Usuarios.FindAsync(id);
                var res = new Response<int>(existingUser.IDusuario, "Usuario actualizado correctamente");

                if (existingUser == null)
                {
                    // Manejar el caso en que el usuario no existe
                    return new Response<int>(0, "Usuario no encontrado");
                }
                // Actualizar los datos del usuario existente con los datos de request
                _mapper.Map(request, existingUser);
                _dbContext.Entry(existingUser).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                //logs
                var log = new logsDTO();
                //log.Datos = request.Nombre + request.Apellido + request.Edad + request.Correo + request.Numero;
                log.Datos = JsonConvert.SerializeObject(request);
                log.Fecha = DateTime.Now.ToString();
                log.ip = GetClientIpAddress().Result.Message;
                log.Status = "200";
                log.Mensaje_ = res.Message;
                log.nombrefuncion = "UpdateUser";

                await GetLogsCreate(log);
                return res;
            }
            catch (Exception ex)
            {
                // Manejar errores y registrar logs
                var errorLog = new logsDTO();
                //errorLog.Datos = request.Nombre + request.Apellido + request.Edad + request.Correo + request.Numero;
                errorLog.Datos = JsonConvert.SerializeObject(request);
                errorLog.Fecha = DateTime.Now.ToString();
                errorLog.ip = GetClientIpAddress().Result.Message;
                errorLog.nombrefuncion = "Update";

                if (ex.InnerException != null)
                {
                    errorLog.Mensaje_ = $"Error desconocido al Actualizar el registro. Mensaje interno: {ex.InnerException.Message}";
                }
                else
                {
                    errorLog.Mensaje_ = "Error desconocido al Actualizar el registro";
                }
                errorLog.Status = "500";
                await GetLogsCreate(errorLog);
                throw;
            }
        }

        public async Task<Response<int>> Deletepersona(int id, UserTDO request)
        {
            try
            {
                var existingUser = await _dbContext.Usuarios.FindAsync(id);
                var res = new Response<int>(existingUser.IDusuario, "Usuario Eliminado correctamente");

                if (existingUser == null)
                {
                    return new Response<int>(0, "Usuario no encontrado");
                }

                _dbContext.Usuarios.Remove(existingUser);
                await _dbContext.SaveChangesAsync();

                //logs
                var log = new logsDTO();
                //log.Datos = request.Nombre + request.Apellido + request.Edad + request.Correo + request.Numero;
                log.Datos = JsonConvert.SerializeObject(request);
                log.Fecha = DateTime.Now.ToString();
                log.ip = GetClientIpAddress().Result.Message;
                log.Status = "200";
                log.Mensaje_ = res.Message;
                log.nombrefuncion = "DeleteUser";

                await GetLogsCreate(log);
                return res;
            }
            catch (Exception ex)
            {
                // Manejar errores y registrar logs
                var errorLog = new logsDTO();
                //errorLog.Datos = request.Nombre + request.Apellido + request.Edad + request.Correo + request.Numero;
                errorLog.Datos = JsonConvert.SerializeObject(request);
                errorLog.Fecha = DateTime.Now.ToString();
                errorLog.ip = GetClientIpAddress().Result.Message;
                errorLog.nombrefuncion = "Delete";

                if (ex.InnerException != null)
                {
                    errorLog.Mensaje_ = $"Error desconocido al eliminar el registro. Mensaje interno: {ex.InnerException.Message}";
                }
                else
                {
                    errorLog.Mensaje_ = $"Error desconocido al eliminar el registro";
                }
                errorLog.Status = "500";
                await GetLogsCreate(errorLog);
                throw;
            }
        }
    }
}