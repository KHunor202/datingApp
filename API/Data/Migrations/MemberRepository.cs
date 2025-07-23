using System;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Migrations;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public void UpdateMember(Member member)
    {
        context.Entry(member).State = EntityState.Modified;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IReadOnlyList<Member>> GetMembersAsync(string username)
    {
        return await context.Members.ToListAsync();
    }

    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await context.Members.FindAsync(id);
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
    {
        return await context.Members.Where(r => r.Id == memberId).SelectMany(r => r.Photos).ToListAsync();
    }
}
