package com.example.fuel.model.page

data class PageRequest(
    val pageNumber: Int?,
    val pageSize: Int?,
    val sortBy: String?,
    val sortDirection: String?
) {

    fun toQueryMap(): Map<String, String> {
        return mapOf(
            "PageNumber" to (pageNumber?.toString() ?: ""),
            "PageSize" to (pageSize?.toString() ?: ""),
            "Sort.SortBy" to (sortBy ?: ""),
            "Sort.SortDirection" to (sortDirection ?: "")
        )
    }
}