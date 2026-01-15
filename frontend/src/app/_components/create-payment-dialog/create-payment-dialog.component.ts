import { Component, Inject, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { PaymentService } from "../../_services/payment.service";
import { Loan } from "../../_models/loan";

@Component({
  selector: "app-create-payment-dialog",
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: "./create-payment-dialog.component.html",
  styleUrl: "./create-payment-dialog.component.scss",
})
export class CreatePaymentDialogComponent implements OnInit {
  form: FormGroup;
  isLoading = false;
  loan: Loan;

  constructor(
    private fb: FormBuilder,
    private paymentService: PaymentService,
    private dialogRef: MatDialogRef<CreatePaymentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { loan: Loan },
  ) {
    this.loan = data.loan;
    this.form = this.fb.group({
      amount: [
        null,
        [
          Validators.required,
          Validators.min(0.01),
          Validators.max(this.loan.currentBalance),
        ],
      ],
    });
  }

  ngOnInit(): void {}

  get maxAmount(): number {
    return this.loan.currentBalance;
  }

  get remainingAfterPayment(): number {
    const paymentAmount = this.form.get("amount")?.value || 0;
    return this.loan.currentBalance - paymentAmount;
  }

  save(): void {
    if (this.form.invalid) return;

    this.isLoading = true;

    const payload = {
      amount: this.form.value.amount,
      idLoan: this.loan.id,
    };

    this.paymentService.createPayment(payload).subscribe({
      next: () => {
        this.isLoading = false;
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        console.error("Error creating payment:", err);
      },
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }

  payFullAmount(): void {
    this.form.patchValue({
      amount: this.loan.currentBalance,
    });
  }
}
