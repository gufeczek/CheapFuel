package com.example.fuel

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.EditText
import android.widget.Toolbar

class RegisterActivity : AppCompatActivity() {

    private lateinit var etPassword: EditText
    private lateinit var toolbar: Toolbar

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)
        supportActionBar?.hide()

        etPassword = findViewById(R.id.registerActivity_et_password)
        toolbar = findViewById(R.id.registerActivity_toolbar)

        CornerDrawable.roundCorners(etPassword, 20f)

        toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
    }
}