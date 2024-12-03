
namespace CGUI
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.guIclass1 = new CGUI.GUIclass();
            this.SuspendLayout();
            // 
            // guIclass1
            // 
            this.guIclass1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.guIclass1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.guIclass1.Location = new System.Drawing.Point(63, 42);
            this.guIclass1.Name = "guIclass1";
            this.guIclass1.Size = new System.Drawing.Size(156, 156);
            this.guIclass1.TabIndex = 0;
            this.guIclass1.Text = "guIclass1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 450);
            this.Controls.Add(this.guIclass1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private GUIclass guIclass1;
    }
}

