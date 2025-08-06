using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ContactsApi.Dtos;
using ContactsApi.Models;
using ContactsApi.Repositories.Abstraction;

namespace ContactsApi.Controllers;

[ApiController, Route("api/[controller]")]
public class ContactController(
    IContactService contactService,
    IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateContactDto dto,
        CancellationToken abortionToken = default)
    {
        var model = mapper.Map<CreateContact>(dto);
        var created = await contactService.CreateContactAsync(model, abortionToken);
        return Ok(mapper.Map<ContactDto>(created));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken abortionToken = default)
    {
        var contacts = await contactService.GetAllAsync(abortionToken);
        return Ok(contacts.Select(mapper.Map<ContactDto>));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken abortionToken = default)
    {
        var single = await contactService.GetSingleAsync(id, abortionToken);
        return Ok(mapper.Map<ContactDto>(single));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateById(
        int id,
        [FromBody] UpdateContactDto dto,
        CancellationToken abortionToken = default)
    {
        var updated = await contactService.UpdateAsync(
            id,
            mapper.Map<UpdateContact>(dto),
            abortionToken);

        return Ok(mapper.Map<ContactDto>(updated));
    }
}