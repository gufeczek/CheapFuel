package com.example.fuel.ui.fragment.user

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.Menu
import android.view.View
import android.view.ViewGroup
import android.widget.PopupMenu
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentUserReviewBinding
import com.example.fuel.enums.Role
import com.example.fuel.utils.Auth
import com.example.fuel.model.review.Review
import com.example.fuel.viewmodel.UserDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.dialog.MaterialAlertDialogBuilder


class UserReviewFragment(private val review: Review) : Fragment() {
    private lateinit var viewModel: UserDetailsViewModel
    private lateinit var binding: FragmentUserReviewBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserDetailsViewModel::class.java]
        binding = FragmentUserReviewBinding.inflate(inflater, container, false)

        initWithData()
        initPopupMenu()

        return binding.root
    }

    private fun initWithData() {
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

        if (review.username != Auth.username && Auth.role != Role.ADMIN) {
            actionButton.visibility = View.GONE
            return
        }

        actionButton.setOnClickListener {
            val popupMenu = PopupMenu(requireActivity(), actionButton)
            initPopupMenuItems(popupMenu)

            popupMenu.show()
        }
    }

    private fun initPopupMenuItems(popupMenu: PopupMenu) {
        val deleteItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 1, resources.getString(R.string.delete))

        deleteItem.setOnMenuItemClickListener {
            askForDeleteConfirmationOfOwnedReview()
            true
        }
    }

    private fun askForDeleteConfirmationOfOwnedReview() {
        MaterialAlertDialogBuilder(requireContext(), R.style.MaterialComponents_MaterialAlertDialog_RoundedCorners)
            .setMessage(getString(R.string.ask_if_delete_review))
            .setPositiveButton(resources.getString(R.string.yes)) { _, _ ->
                viewModel.deleteUserReview(review.id)
            }
            .setNegativeButton(resources.getString(R.string.no), null)
            .show()
    }
}