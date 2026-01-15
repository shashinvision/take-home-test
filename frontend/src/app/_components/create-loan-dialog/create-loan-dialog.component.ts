import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatSelectModule } from "@angular/material/select";
import { LoanService } from "../../_services/loan.service";
import { Applicants } from "../../_models/applicants";
import { loanPayload } from "../../_models/loanPayload";

@Component({
  selector: "app-create-loan-dialog",
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
  ],
  templateUrl: "./create-loan-dialog.component.html",
  styleUrl: "./create-loan-dialog.component.scss",
})
export class CreateLoanDialogComponent implements OnInit {
  form: FormGroup;
  applicants: Applicants[] = [];
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private loanService: LoanService,
    private dialogRef: MatDialogRef<CreateLoanDialogComponent>,
  ) {
    this.form = this.fb.group({
      amount: [null, [Validators.required, Validators.min(1)]],
      currentBalance: [null, [Validators.required, Validators.min(0)]],
      applicantId: [null, Validators.required],
      isActive: [true],
    });
  }

  ngOnInit(): void {
    this.loadApplicants();
  }

  loadApplicants(): void {
    this.loanService.getApplicants().subscribe({
      next: (response: any) => {
        console.log("Applicants response:", response);
        this.applicants = response.data || [];
      },
      error: (err) => {
        console.error("Error loading applicants:", err);
      },
    });
  }

  save(): void {
    if (this.form.invalid) return;

    this.isLoading = true;

    // Transformar el payload para que coincida con el backend
    const payload: loanPayload = {
      amount: this.form.value.amount,
      currentBalance: this.form.value.currentBalance,
      isActive: this.form.value.isActive ? 1 : 0,
      idApplicant: this.form.value.applicantId,
    };

    this.loanService.createLoan(payload).subscribe({
      next: () => {
        this.isLoading = false;
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        console.error("Error creating loan:", err);
      },
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
