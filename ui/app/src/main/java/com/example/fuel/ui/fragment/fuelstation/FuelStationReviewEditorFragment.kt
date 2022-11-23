package com.example.fuel.ui.fragment.fuelstation

import android.app.Dialog
import android.content.res.Resources
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.WindowManager
import androidx.appcompat.widget.AppCompatRatingBar
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.core.widget.addTextChangedListener
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.model.review.Review
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.bottomsheet.BottomSheetBehavior
import com.google.android.material.bottomsheet.BottomSheetDialog
import com.google.android.material.bottomsheet.BottomSheetDialogFragment
import com.google.android.material.button.MaterialButton
import com.google.android.material.textfield.TextInputEditText


class FuelStationReviewEditorFragment(val review: Review?, private val editing: Boolean) : BottomSheetDialogFragment() {
    private lateinit var fuelStationReviewEditorFragment: View
    private lateinit var viewModel: FuelStationDetailsViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        fuelStationReviewEditorFragment =  inflater.inflate(R.layout.fragment_fuel_station_review_editor, container, false)

        setInitialData()
        initPublishButton()
        initRatingBar()
        initReviewContentTextInput()
        initKeyboardHiding()

        return fuelStationReviewEditorFragment
    }

    override fun onCreateDialog(savedInstanceState: Bundle?): Dialog {
        val dialog = BottomSheetDialog(requireContext(), theme)
        dialog.setOnShowListener {
            val bottomSheetDialog = it as BottomSheetDialog
            val parentLayout = bottomSheetDialog.findViewById<View>(com.google.android.material.R.id.design_bottom_sheet)
            parentLayout?.let {
                val behavior = BottomSheetBehavior.from(it)
                setupPeekHeight(behavior)
                setupFullHeight(it)
                behavior.state = BottomSheetBehavior.STATE_EXPANDED
            }
        }
        return dialog
    }

    private fun setupFullHeight(bottomSheetDialog: View) {
        val layoutParams = bottomSheetDialog.layoutParams
        layoutParams.height = WindowManager.LayoutParams.MATCH_PARENT
        bottomSheetDialog.layoutParams = layoutParams
    }

    private fun setupPeekHeight(bottomSheetDialog: BottomSheetBehavior<View>) {
        val maxHeight = Resources.getSystem().displayMetrics.heightPixels
        bottomSheetDialog.peekHeight = maxHeight
    }

    private fun initRatingBar() {
        val ratingBar = fuelStationReviewEditorFragment.findViewById<AppCompatRatingBar>(R.id.acrb_rateFuelStation)
        ratingBar.setOnRatingBarChangeListener { _, _, _ -> checkRequiredFields() }
    }

    private fun initReviewContentTextInput() {
        val contentInput = fuelStationReviewEditorFragment.findViewById<TextInputEditText>(R.id.tiet_reviewContent)
        contentInput.addTextChangedListener { checkRequiredFields() }
    }

    private fun setInitialData() {
        if (review == null) return

        setInitialRate(review.rate)
        setInitialContent(review.content)
    }

    private fun setInitialRate(rate: Int) {
        val ratingBar = fuelStationReviewEditorFragment.findViewById<AppCompatRatingBar>(R.id.acrb_rateFuelStation)
        ratingBar.rating = rate.toFloat()
    }

    private fun setInitialContent(content: String?) {
        if (content == null) return

        val contentInput = fuelStationReviewEditorFragment.findViewById<TextInputEditText>(R.id.tiet_reviewContent)
        contentInput.setText(content)
    }

    private fun initPublishButton() {
        val button = fuelStationReviewEditorFragment.findViewById<MaterialButton>(R.id.mb_publishReview)
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
        val main = fuelStationReviewEditorFragment.findViewById<ConstraintLayout>(R.id.cl_reviewEditorContainer)
        main.setOnClickListener { view -> view.hideKeyboard() }
    }

    private fun getRate(): Int {
        val ratingBar = fuelStationReviewEditorFragment.findViewById<AppCompatRatingBar>(R.id.acrb_rateFuelStation)
        return ratingBar.rating.toInt()
    }

    private fun getContent(): String {
        val contentInput = fuelStationReviewEditorFragment.findViewById<TextInputEditText>(R.id.tiet_reviewContent)
        return contentInput.text.toString()
    }

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
        val button = fuelStationReviewEditorFragment.findViewById<MaterialButton>(R.id.mb_publishReview)
        button.isEnabled = true
    }

    private fun disablePublishButton() {
        val button = fuelStationReviewEditorFragment.findViewById<MaterialButton>(R.id.mb_publishReview)
        button.isEnabled = false
    }

    companion object {
        const val TAG = "FuelStationReviewEditor"
    }
}