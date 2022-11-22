package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.Menu
import android.view.View
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.PopupMenu
import android.widget.TextView
import androidx.appcompat.widget.AppCompatRatingBar
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.mock.Auth
import com.example.fuel.model.review.Review
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.dialog.MaterialAlertDialogBuilder

class FuelStationReviewFragment(private val review: Review) : Fragment() {
    private lateinit var viewModel: FuelStationDetailsViewModel
    private lateinit var fuelStationReviewView: View

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        fuelStationReviewView = inflater.inflate(R.layout.fragment_fuel_station_review, container, false)

        initWithData()
        initPopupMenu()

        return fuelStationReviewView
    }

    private fun initWithData() {
        val reviewAuthorTextView = fuelStationReviewView.findViewById<TextView>(R.id.tv_review_author)
        reviewAuthorTextView.text = review.username

        val ratingBar = fuelStationReviewView.findViewById<AppCompatRatingBar>(R.id.acrb_review_rating)
        ratingBar.rating = review.rate.toFloat()

        val createdAtTextView = fuelStationReviewView.findViewById<TextView>(R.id.tv_review_created_at)
        createdAtTextView.text = viewModel.parseReviewDate(review.createdAt, resources)

        if (viewModel.hasReviewContent(review)) {
            val contentTextView = fuelStationReviewView.findViewById<TextView>(R.id.tv_review_content)
            contentTextView.text = review.content
            contentTextView.visibility = View.VISIBLE
        }

        if (viewModel.hasReviewBeenEdited(review)) {
            val updatedAtTextView = fuelStationReviewView.findViewById<TextView>(R.id.tv_review_updated_at)
            updatedAtTextView.text = resources.getString(R.string.edited, viewModel.parseReviewDate(review.updatedAt, resources))
            updatedAtTextView.visibility = View.VISIBLE
        }
    }

    private fun initPopupMenu() {
        val actionButton = fuelStationReviewView.findViewById<ImageButton>(R.id.acib_reviewActionButton)

        actionButton.setOnClickListener {
            val popupMenu = PopupMenu(requireActivity(), actionButton)

            if (review.username == Auth.username) {
                initActionButtonForReviewAuthor(popupMenu)
            } else {
                initActionButtonForReviewOfDifferentUser(popupMenu)
            }

            popupMenu.show()
        }
    }

    private fun initActionButtonForReviewAuthor(popupMenu: PopupMenu) {
        val editItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 0, resources.getString(R.string.edit))
        val deleteItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 1, resources.getString(R.string.delete))

        editItem.setOnMenuItemClickListener {
            openReviewEditor()
            true
        }
        deleteItem.setOnMenuItemClickListener {
            askForDeleteConfirmation()
            true
        }
    }

    private fun initActionButtonForReviewOfDifferentUser(popupMenu: PopupMenu) {
        // TODO: Should be implemented after adding to backend feature to report other users reviews
    }

    private fun openReviewEditor() {
        val reviewEditorFragment = FuelStationReviewEditorFragment(viewModel.userReview.value?.body(), true)
        reviewEditorFragment.show(requireFragmentManager(), FuelStationReviewEditorFragment.TAG)
    }

    private fun askForDeleteConfirmation() {
        MaterialAlertDialogBuilder(requireContext(), R.style.MaterialComponents_MaterialAlertDialog_RoundedCorners)
            .setMessage(resources.getString(R.string.ask_if_delete))
            .setPositiveButton(resources.getString(R.string.yes)) { _, _ ->
                viewModel.deleteUserReview()
            }
            .setNegativeButton(resources.getString(R.string.no), null)
            .show()
    }
}