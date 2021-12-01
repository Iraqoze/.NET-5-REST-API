using System;
using Catalog.Api.Dtos;
using Catalog.Api.Models;

namespace Catalog.Api.Extensions
{
    public static class Extension
    {

        public static ItemDto AsDto(this Item item) =>
         new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreationTime);

    }
}