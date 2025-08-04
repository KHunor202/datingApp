using API.Entities;
using API.Helpers;
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

    public async Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams)
    {
        var query = context.Members.AsQueryable();

        query = query.Where(r => r.Id != memberParams.CurrentMemberId);

        query = memberParams.OrderBy switch
        {
            "created" => query.OrderByDescending(r => r.Created),
            _ => query.OrderByDescending(r => r.LastActive)
        };

        if (memberParams.Gender != null)
        {
            query = query.Where(r => r.Gender == memberParams.Gender);
        }

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MinAge));

        query = query.Where(r => r.DateOfBirth >= minDob && r.DateOfBirth <= maxDob);

        return await PaginationHelper.CreateAsync(query, memberParams.PageNumber, memberParams.PageSize);
    }

    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await context.Members.FindAsync(id);
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
    {
        return await context.Members.Where(r => r.Id == memberId).SelectMany(r => r.Photos).ToListAsync();
    }

    public async Task<Member?> GetMemberForUpdateAsync(string id)
    {
        return await context.Members
            .Include(r => r.User)
            .Include(r => r.Photos)
            .SingleOrDefaultAsync(r => r.Id == id);
    }
}
