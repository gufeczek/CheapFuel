package com.example.fuel.ui.fragment

import android.content.res.ColorStateList
import android.graphics.Color
import android.os.Bundle
import android.text.method.LinkMovementMethod
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetUsernameBinding
import com.example.fuel.ui.utils.TextViewExtension.Companion.removeLinksUnderline
import kotlin.time.Duration.Companion.seconds

class SetUsernameFragment : Fragment() {

    private lateinit var binding: FragmentSetUsernameBinding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentSetUsernameBinding.inflate(inflater, container, false)
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener {
            findNavController().popBackStack()
        }
        binding.tvTermsOfUse.movementMethod = LinkMovementMethod.getInstance()
        binding.tvTermsOfUse.removeLinksUnderline()


        binding.btnNextPage.setOnClickListener {
            val (isValidationPassed: Boolean, msg: String) = validateUsername(binding.etUsername.text.toString())
            if (isValidationPassed) {
                binding.tilUsername.setBackgroundResource(R.drawable.bg_rounded_error)
            }
            else if (!binding.chkTermsOfUse.isChecked) {
                binding.tvTermsOfUseError.visibility = View.VISIBLE
                binding.chkTermsOfUse.buttonTintList = ColorStateList.valueOf(Color.RED)
                binding.tvTermsOfUse.setLinkTextColor(Color.RED)
            }
        }
        binding.chkTermsOfUse.setOnCheckedChangeListener { _, isChecked ->
            if (isChecked) {
                binding.tvTermsOfUseError.visibility = View.GONE
                binding.chkTermsOfUse.buttonTintList = ColorStateList.valueOf(resources.getColor(R.color.checkbox, activity?.theme))
                binding.tvTermsOfUse.setLinkTextColor(resources.getColor(R.color.checkbox, activity?.theme))
            }
        }

        return binding.root
    }

    private fun validateUsername(username: String): Pair<Boolean, String> {
        Log.d("XD", "VALIDATION: username = ${username}")
        if (username.length < 3) {
            return Pair(false, "Nazwa użytkownika jest zbyt krótka")
        } else if (username.length > 32) {
            return Pair(false, "Nazwa użytkownika jest zbyt długa")
        }
        return Pair(true, "")
    }
}