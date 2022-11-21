package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.appcompat.widget.AppCompatRatingBar
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.model.review.Review
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class FuelStationCommentFragment(private val review: Review) : Fragment() {
    private lateinit var viewModel: FuelStationDetailsViewModel
    private lateinit var fuelStationCommentView: View

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        fuelStationCommentView = inflater.inflate(R.layout.fragment_fuel_station_comment, container, false)

        initWithData()

        return fuelStationCommentView
    }

    private fun initWithData() {
        val reviewAuthorTextView = fuelStationCommentView.findViewById<TextView>(R.id.tv_review_author)
        reviewAuthorTextView.text = review.username

        val ratingBar = fuelStationCommentView.findViewById<AppCompatRatingBar>(R.id.acrb_review_rating)
        ratingBar.rating = review.rate.toFloat()

        val createdAtTextView = fuelStationCommentView.findViewById<TextView>(R.id.tv_review_created_at)
        createdAtTextView.text = viewModel.parseReviewDate(review.createdAt, resources)

        if (viewModel.hasReviewContent(review)) {
            val contentTextView = fuelStationCommentView.findViewById<TextView>(R.id.tv_review_content)
            contentTextView.text = review.content
            contentTextView.visibility = View.VISIBLE
        }

        if (viewModel.hasReviewBeenEdited(review)) {
            val updatedAtTextView = fuelStationCommentView.findViewById<TextView>(R.id.tv_review_updated_at)
            updatedAtTextView.text = resources.getString(R.string.edited, viewModel.parseReviewDate(review.updatedAt, resources))
            updatedAtTextView.visibility = View.VISIBLE
        }
    }
}