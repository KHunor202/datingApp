using System;
using API.Entities;

namespace API.Interfaces;

public interface IMemberRepository
{
    void UpdateMember(Member member);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>> GetMembersAsync(string username);
    Task<Member?> GetMemberByIdAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId);
    Task<Member?> GetMemberForUpdateAsync(string id);
}
