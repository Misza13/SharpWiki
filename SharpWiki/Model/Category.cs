﻿namespace SharpWiki.Model
{
    using System.Collections.Generic;
    using API.Queries;
    using SharpWiki.Model.Queries;

    public class Category : Page
    {
        internal Category(MediaWikiSite site, int namespaceId, string title) : base(site, namespaceId, title)
        {
        }

        /// <summary>
        /// Get all pages that are contained in this category.
        /// </summary>
        /// <returns></returns>
        public ICategoryMembersQuery GetMembers()
        {
            return new CategoryMembersQuery(this.Site, this);
        }
    }
}