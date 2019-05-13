using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WidgetExample.Dto;
using WidgetExample.Entities;

namespace WidgetExample.Data {
  public class WidgetRepo : IWidgetRepo {
    private readonly DataContext _context;

    public WidgetRepo(DataContext context) {
      _context = context;
    }

    public async Task<bool> AddToken(AccessTokenDto accessTokenDto) {
      var token = new AccessToken {Level = accessTokenDto.Level, Token = accessTokenDto.Token};
      _context.AccessTokens.Add(token);
      var result = await _context.SaveChangesAsync();
      return result > 0;
    }

    public async Task<IEnumerable<AccessToken>> ListTokens() {
      return await _context.AccessTokens.ToListAsync();
    }

    public async Task<bool> DeleteToken(int id) {
      var token = await _context.AccessTokens.FirstOrDefaultAsync(t => t.Id == id);
      if (token == null) {
        return false;
      }

      _context.AccessTokens.Remove(token);
      var result = await _context.SaveChangesAsync();
      return result > 0;
    }

    public async Task<AccessToken> UpdateToken(AccessToken accessToken) {
      var token = await _context.AccessTokens.FirstOrDefaultAsync(t => t.Id == accessToken.Id);
      if (token == null) {
        return null;
      }

      token.Level = accessToken.Level;
      token.Token = accessToken.Token;
      _context.AccessTokens.Add(token);
      var result = await _context.SaveChangesAsync();
      return result > 0 ? token : null;
    }

    public Task<Widget> Add(Widget widget) {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Widget>> GetAll() {
      throw new NotImplementedException();
    }

    public Task<Widget> Get(string id) {
      throw new NotImplementedException();
    }

    public Task<bool> Remove(string id) {
      throw new NotImplementedException();
    }

    public Task<Widget> Update(Widget widget) {
      throw new NotImplementedException();
    }
  }
}