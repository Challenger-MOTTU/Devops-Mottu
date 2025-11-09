using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Challenger.Application.DTOs.Requests;
using Challenger.Application.DTOs.Responses;
using Challenger.Application.pagination;
using Challenger.Application.UseCase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenger.Domain.Entities;
using Challenger.Domain.Interfaces;
using Challenger.Infrastructure.Context;

namespace WebApplication2.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        ICreateUserUseCase createUserUseCase,
        IUpdateUserUseCase updateUserUseCase,
        IUserRepository userRepository)
        : ControllerBase
    {
        // GET: api/User
        /// <summary>
        ///  Busca todos os usuarios Cadastrados
        /// </summary>
        /// <returns>
        ///  retorna 200 (OK) com a lista de usuarios
        /// </returns>
        /// <remarks>
        /// Exemplo de resposta (200 Ok):
        ///
        ///        [
        ///             {
        ///                 "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                 "username": "Dustsams",
        ///                 "email": "davichugopereira@gmail.com",
        ///                 "senha": "Fiapm&5F"
        ///             }
        ///         ]
        /// </remarks>
        /// 
        [HttpGet]
        public async Task<IEnumerable<UserResponse >> GetUsers()
        {
            var users = await userRepository.GetAllAsync();
            return users.Select(u => new UserResponse(
                u.Id, 
                u.Username, 
                u.Email.Valor, 
                u.Senha.Valor
            ));
        }

        [HttpGet("paged")]
        public Task<PaginatedResult<UserSummary>> GetPage([FromQuery] PageRequest pageRequest,
            [FromQuery] UserQuery userQuery)
        {
            return createUserUseCase.ExecuteAsync(pageRequest, userQuery);
        }
        

        // GET: api/User/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">O ID do Usuario a ser consultado</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if(user == null) return NotFound("Usuario não encontrado");
            return new UserResponse(
                user.Id,
                user.Username,
                user.Email.Valor,
                user.Senha.Valor
            );
        }
        
        
        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<User>> PostUser(UserRequest request)
        {
            try
            {
                string createdBy = User?.Identity?.Name ?? "system";
                var userResponse = await createUserUseCase.Execute(request, createdBy);
                return CreatedAtAction(nameof(GetUser), new { id = userResponse.Id }, userResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro a cadastrar Usuario" });
            }
        }


        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> PutUser(Guid id, UserRequest request)
        {
            try
            {
                string updatedBy = User?.Identity?.Name ?? "system";
                var updated = await updateUserUseCase.Execute(id, request, updatedBy);

                if (!updated) return NotFound("Usuário não encontrado");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao atualizar Usuario" });
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null) return NotFound("Usuário não encontrado");
            
            await userRepository.DeleteAsync(user);
            return NoContent();
        }
        
    }
}
