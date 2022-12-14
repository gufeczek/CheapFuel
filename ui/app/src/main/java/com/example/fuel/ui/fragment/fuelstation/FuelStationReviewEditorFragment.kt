package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.widget.addTextChangedListener
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.databinding.FragmentFuelStationReviewEditorBinding
import com.example.fuel.model.review.Review
import com.example.fuel.ui.fragment.common.FullHeightBottomSheetDialogFragment
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory


class FuelStationReviewEditorFragment(val review: Review?, private val editing: Boolean) : FullHeightBottomSheetDialogFragment() {
    private lateinit var viewModel: FuelStationDetailsViewModel
    private lateinit var binding: FragmentFuelStationReviewEditorBinding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        binding = FragmentFuelStationReviewEditorBinding.inflate(inflater, container, false)

        setInitialData()
        initPublishButton()
        initRatingBar()
        initReviewContentTextInput()
        initKeyboardHiding()

        return binding.root
    }

    private fun initRatingBar() {
        val ratingBar = binding.acrbRateFuelStation
        ratingBar.setOnRatingBarChangeListener { _, _, _ -> checkRequiredFields() }
    }

    private fun initReviewContentTextInput() {
        val contentInput = binding.tietReviewContent
        contentInput.addTextChangedListener { checkRequiredFields() }
    }

    private fun setInitialData() {
        if (review == null) return

        setInitialRate(review.rate)
        setInitialContent(review.content)
    }

    private fun setInitialRate(rate: Int) {
        val ratingBar = binding.acrbRateFuelStation
        ratingBar.rating = rate.toFloat()
    }

    private fun setInitialContent(content: String?) {
        if (content == null) return

        val contentInput = binding.tietReviewContent
        contentInput.setText(content)
    }

    private fun initPublishButton() {
        val button = binding.mbPublishReview
        button.isEnabled = false

        button.setOnClickListener {
            val rating = getRate()
            val content = getContent()

            if (editing) {
                viewModel.editUserReview(rating, content)
            } else {
                viewModel.createNewReviewForUser(rating, content)
            }

            dismiss()
        }
    }

    private fun initKeyboardHiding() {
        val main = binding.clReviewEditorContainer
        main.setOnClickListener { view -> view.hideKeyboard() }
    }

    private fun getRate(): Int = binding.acrbRateFuelStation.rating.toInt()

    private fun getContent(): String = binding.tietReviewContent.text.toString()

    private fun checkRequiredFields() {
        val rate = getRate()
        val content = getContent()

        if (viewModel.isReviewValid(rate, content)) {
            enablePublishButton()
        } else {
            disablePublishButton()
        }
    }

    private fun enablePublishButton() {
            binding.mbPublishReview.isEnabled = true
    }

    private fun disablePublishButton() {
        binding.mbPublishReview.isEnabled = false
    }

    companion object {
        const val TAG = "FuelStationReviewEditor"
    }
}