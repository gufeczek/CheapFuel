package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.Menu
import android.view.View
import android.view.ViewGroup
import android.widget.PopupMenu
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationReviewBinding
import com.example.fuel.utils.Auth
import com.example.fuel.model.review.Review
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.dialog.MaterialAlertDialogBuilder

class FuelStationReviewFragment(private val review: Review) : Fragment() {
    private lateinit var viewModel: FuelStationDetailsViewModel
    private lateinit var binding: FragmentFuelStationReviewBinding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        binding = FragmentFuelStationReviewBinding.inflate(inflater, container, false)

        initWithData()
        initPopupMenu()

        return binding.root
    }

    private fun initWithData() {
        binding.tvReviewAuthor.text = review.username
        binding.acrbReviewRating.rating = review.rate.toFloat()
        binding.tvReviewCreatedAt.text = viewModel.parseReviewDate(review.createdAt, resources)

        if (viewModel.hasReviewContent(review)) {
            val contentTextView = binding.tvReviewContent
            contentTextView.text = review.content
            contentTextView.visibility = View.VISIBLE
        }

        if (viewModel.hasReviewBeenEdited(review)) {
            val updatedAtTextView = binding.tvReviewUpdatedAt
            updatedAtTextView.text = resources.getString(R.string.edited, viewModel.parseReviewDate(review.updatedAt, resources))
            updatedAtTextView.visibility = View.VISIBLE
        }
    }

    private fun initPopupMenu() {
        val actionButton = binding.acibReviewActionButton

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
            openReviewEditor(viewModel.userReview.value?.body())
            true
        }
        deleteItem.setOnMenuItemClickListener {
            askForDeleteConfirmationOfOwnedReview()
            true
        }
    }

    private fun initActionButtonForReviewOfDifferentUser(popupMenu: PopupMenu) {
        if (viewModel.isAdmin()) {
            initActionButtonForAdmin(popupMenu)
        } else {
            initActionButtonForUser(popupMenu)
        }
    }

    private fun initActionButtonForUser(popupMenu: PopupMenu) {
        val reportItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 1, resources.getString(R.string.report))

        reportItem.setOnMenuItemClickListener {
            askForReportConfirmation()
            true
        }
    }

    private fun initActionButtonForAdmin(popupMenu: PopupMenu) {
        val deleteItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 1, resources.getString(R.string.delete))

        deleteItem.setOnMenuItemClickListener {
            askForDeleteConfirmationOfDifferentUserReview()
            true
        }
    }

    private fun openReviewEditor(review: Review?) {
        val reviewEditorFragment = FuelStationReviewEditorFragment(review, true)
        reviewEditorFragment.show(requireFragmentManager(), FuelStationReviewEditorFragment.TAG)
    }

    private fun askForDeleteConfirmationOfOwnedReview() {
        MaterialAlertDialogBuilder(requireContext(), R.style.MaterialComponents_MaterialAlertDialog_RoundedCorners)
            .setMessage(resources.getString(R.string.ask_if_delete))
            .setPositiveButton(resources.getString(R.string.yes)) { _, _ ->
                viewModel.deleteUserReview()
            }
            .setNegativeButton(resources.getString(R.string.no), null)
            .show()
    }

    private fun askForDeleteConfirmationOfDifferentUserReview() {
        MaterialAlertDialogBuilder(requireContext(), R.style.MaterialComponents_MaterialAlertDialog_RoundedCorners)
            .setMessage(resources.getString(R.string.ask_if_delete_diff_user_review, review.username))
            .setPositiveButton(resources.getString(R.string.yes)) { _, _ ->
                viewModel.deleteUserReview(review.id)
            }
            .setNegativeButton(resources.getString(R.string.no), null)
            .show()
    }

    private fun askForReportConfirmation() {
        MaterialAlertDialogBuilder(requireContext(), R.style.MaterialComponents_MaterialAlertDialog_RoundedCorners)
            .setMessage(resources.getString(R.string.ask_if_report_user, review.username))
            .setPositiveButton(resources.getString(R.string.yes)) { _, _ ->
                viewModel.reportReview(review.id)
            }
            .setNegativeButton(resources.getString(R.string.no), null)
            .show()
    }
}