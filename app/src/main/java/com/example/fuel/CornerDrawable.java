package com.example.fuel;

import android.content.res.ColorStateList;
import android.graphics.drawable.ColorDrawable;
import android.graphics.drawable.Drawable;
import android.graphics.drawable.GradientDrawable;
import android.graphics.drawable.RippleDrawable;
import android.util.Log;
import android.view.View;
import android.widget.Button;

import androidx.annotation.NonNull;
import androidx.appcompat.widget.TintInfo;

import java.lang.reflect.Field;

import lombok.AccessLevel;
import lombok.NoArgsConstructor;

@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class CornerDrawable extends GradientDrawable {

    private float percent;

    public static void roundCorners(View view, float percent) {
        CornerDrawable drawable = new CornerDrawable();
        drawable.setCornerRadiusPercent(percent);
        drawable.setColor(getColorFromView(view));
        view.setBackground(drawable);
    }

    private static int getColorFromView(View view) {
        ColorDrawable colorDrawable = (ColorDrawable) view.getBackground();
        return colorDrawable.getColor();
    }

    public static int getButtonBackgroundColor(Button button){
        int buttonColor = 0;

        if (button.getBackground() instanceof ColorDrawable) {
            ColorDrawable cd = (ColorDrawable) button.getBackground();
            buttonColor = cd.getColor();
        }

        if (button.getBackground() instanceof RippleDrawable) {
            RippleDrawable rippleDrawable = (RippleDrawable) button.getBackground();
            Drawable.ConstantState state = rippleDrawable.getConstantState();
            try {
                Field colorField = state.getClass().getDeclaredField("mColor");
                colorField.setAccessible(true);
                ColorStateList colorStateList = (ColorStateList) colorField.get(state);
                buttonColor = colorStateList.getDefaultColor();
            } catch (NoSuchFieldException e) {
                e.printStackTrace();
            } catch (IllegalAccessException e) {
                e.printStackTrace();
            }
        }
        return buttonColor;
    }

    private void setCornerRadiusPercent(float percent) {
        this.percent = 100 / percent;
    }

    @Override
    public void setBounds(int left, int top, int right, int bottom) {
        super.setBounds(left, top, right, bottom);
        setCornerRadius(getBounds().height() / percent);
    }

    @Override
    public void setBounds(@NonNull android.graphics.Rect bounds) {
        super.setBounds(bounds);
        setCornerRadius(getBounds().height() / percent);
    }


}