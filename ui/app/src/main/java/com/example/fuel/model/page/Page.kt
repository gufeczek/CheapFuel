package com.example.fuel.model.page

data class Page<T>(
    val pageNumber: Int,
    val pageSize: Int,
    val nextPage: Int?,
    val previousPage: Int?,
    val firstPage: Int,
    val lastPage: Int?,
    val totalPages: Int,
    val totalElements: Long,
    val sort: Sort?,
    val data: Array<T>
) {
    override fun equals(other: Any?): Boolean {
        if (this === other) return true
        if (javaClass != other?.javaClass) return false

        other as Page<*>

        if (pageNumber != other.pageNumber) return false
        if (pageSize != other.pageSize) return false
        if (nextPage != other.nextPage) return false
        if (previousPage != other.previousPage) return false
        if (firstPage != other.firstPage) return false
        if (lastPage != other.lastPage) return false
        if (totalPages != other.totalPages) return false
        if (totalElements != other.totalElements) return false
        if (sort != other.sort) return false
        if (!data.contentEquals(other.data)) return false

        return true
    }

    override fun hashCode(): Int {
        var result = pageNumber
        result = 31 * result + pageSize
        result = 31 * result + (nextPage ?: 0)
        result = 31 * result + (previousPage ?: 0)
        result = 31 * result + firstPage
        result = 31 * result + (lastPage ?: 0)
        result = 31 * result + totalPages
        result = 31 * result + totalElements.hashCode()
        result = 31 * result + (sort?.hashCode() ?: 0)
        result = 31 * result + data.contentHashCode()
        return result
    }
}