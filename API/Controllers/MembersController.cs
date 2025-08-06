using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    public class MembersController(IUnitOfWork uow, IPhotoService photoService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers([FromQuery] MemberParams memberParams)
        {
            memberParams.CurrentMemberId = User.GetMemberId();

            return Ok(await uow.MemberRepository.GetMembersAsync(memberParams));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await uow.MemberRepository.GetMemberByIdAsync(id);
            if (member == null)
                return NotFound();

            return member;
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetPhotos(string id)
        {
            return Ok(await uow.MemberRepository.GetPhotosForMemberAsync(id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();

            var member = await uow.MemberRepository.GetMemberForUpdateAsync(memberId);
            if (member == null)
                return BadRequest("Could not find member");

            member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
            member.Description = memberUpdateDto.Description ?? member.Description;
            member.City = memberUpdateDto.City ?? member.City;
            member.Country = memberUpdateDto.Country ?? member.Country;

            member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;

            // memberRepository.UpdateMember(member);

            if (await uow.Complete())
                return NoContent();

            return BadRequest("Failed to update the member");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm] IFormFile file)
        {
            var member = await uow.MemberRepository.GetMemberForUpdateAsync(User.GetMemberId());

            if (member == null)
                return BadRequest("Member not found");

            var result = await photoService.UploadPhotoAsync(file);

            if (result.Error != null)
                return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;
            }

            member.Photos.Add(photo);

            if (await uow.Complete())
                return photo;

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var member = await uow.MemberRepository.GetMemberForUpdateAsync(User.GetMemberId());

            if (member == null)
                return BadRequest("Member not found");

            var photo = member.Photos.SingleOrDefault(r => r.Id == photoId);
            if (member.ImageUrl == photo?.Url || photo == null)
                return BadRequest("Cannot set this photo as main");

            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;

            if (await uow.Complete())
                return NoContent();

            return BadRequest("Problem setting main photo");
        }
        
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)    
        {
            var member = await uow.MemberRepository.GetMemberForUpdateAsync(User.GetMemberId());
            if (member == null)
                return BadRequest("Member not found");

            var photo = member.Photos.SingleOrDefault(r => r.Id == photoId);

            if (photo == null || photo.Url == member.ImageUrl)
                return BadRequest("Cannot delete this photo");
        
            if(photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                    return BadRequest(result.Error.Message);
            }

            member.Photos.Remove(photo);

            if (await uow.Complete())
                return NoContent();

            return BadRequest("Problem deleting photo");
        }
    }
}
