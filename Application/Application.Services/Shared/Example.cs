using Application.Domain;
using Application.Services.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Shared
{
    class Example
    {
        public Example()
        {
            var items = new List<Item>()
            {
                new Item()
                {
                    Id = 1,
                    ParentId = 1,
                    HasParent = false,
                    LevelInHierarchy = 1
                },
                new Item()
                {
                    Id = 2,
                    ParentId = 2,
                    HasParent = false,
                    LevelInHierarchy = 1
                },
                new Item()
                {
                    Id = 3,
                    ParentId = 3,
                    HasParent = false,
                    LevelInHierarchy = 1
                },
                new Item()
                {
                    Id = 4,
                    ParentId = 4,
                    HasParent = false,
                    LevelInHierarchy = 1
                },
                new Item()
                {
                    Id = 11,
                    ParentId = 1,
                    HasParent = true,
                    LevelInHierarchy = 2
                },
                new Item()
                {
                    Id = 12,
                    ParentId = 1,
                    HasParent = true,
                    LevelInHierarchy = 2
                },
                new Item()
                {
                    Id = 111,
                    ParentId = 11,
                    HasParent = true,
                    LevelInHierarchy = 3
                },
                new Item()
                {
                    Id = 21,
                    ParentId = 2,
                    HasParent = true,
                    LevelInHierarchy = 2
                },
            };

            var x = new FlattenHierarchyService<Item, int>(items).Flatten(IsEqual);

            var y = false;
        }

        public static bool IsEqual(int x, int y)
        {
            return x == y;
        }
    }

    class Item : IHierarchyItem<int>
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int LevelInHierarchy { get; set; }
        public bool HasParent { get; set; }
    }
}
