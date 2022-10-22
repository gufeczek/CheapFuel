package com.example.fuel.ui.login

import android.os.Bundle
import android.widget.EditText
import android.widget.Toolbar
import androidx.appcompat.app.AppCompatActivity
import com.example.fuel.R
import com.example.fuel.ui.login.fragment.SetPasswordFragment

class RegisterActivity : AppCompatActivity() {

    private lateinit var toolbar: Toolbar
    private lateinit var etPassword: EditText

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)
        supportActionBar?.hide()

        toolbar = findViewById(R.id.registerActivity_toolbar)
        toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)

        supportFragmentManager
            .beginTransaction()
            .replace(R.id.frameLayout, SetPasswordFragment())
            .commit()

    }


}