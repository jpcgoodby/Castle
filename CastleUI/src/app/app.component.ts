import { ChangeDetectorRef } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { timer } from 'rxjs';
import { AppService } from './app.service';
import { Castle } from './castle';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'CastleUI';
  loading: boolean = true;
  displayedColumns: string[] = ['name', 'url', 'location', 'country', 'dateBuilt'];
  dataSource = new MatTableDataSource([]);
  catleForm: FormGroup;
  numericPattern = '^-?[0-9]\\d*(\\.\\d{1,2})?$';

  constructor(private appService: AppService, 
              private formBuilder: FormBuilder){
    this.build();
  }

  ngOnInit(): void {
    this.appService.get().subscribe((castles: Array<any>) => {

      timer(1000).subscribe(t => {
        this.dataSource = new MatTableDataSource(castles);
        this.loading = false;
      });
      
    });
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  sortData(sort: Sort) {
    const data = this.dataSource.data.slice();

    if (!sort.active || sort.direction === '') {
      this.dataSource.data = data;
    } else {
      this.dataSource.data = data.sort((a, b) => {
        const aValue = (a as any)[sort.active];
        const bValue = (b as any)[sort.active];
        return (aValue < bValue ? -1 : 1) * (sort.direction === 'asc' ? 1 : -1);
      });
    }
  }

  submit() {
    if (this.catleForm.valid) {
      this.loading = true;
     
      let formData = new FormData();
      formData[0] = {'file':  this.catleForm.get('fileSource').value};
  
      formData.forEach((value, key) => {
        console.log(key + " " + value)
      });
      formData.append('file', this.catleForm.get('fileSource').value);

      this.appService.upload(formData).subscribe((result: any) => {
        const payload = Object.assign(new Castle(), this.catleForm.value);

        this.appService.create(payload).subscribe((result: any) => {
          this.appService.get().subscribe((castles: Array<any>) => {
            timer(1000).subscribe(t => {
              this.dataSource = new MatTableDataSource(castles);
              this.Cancel();
              this.loading = false;
            }); 
          },
          (error: any) => {
            this.loading = false;
          });
        });
      },
      (error: any) => {
        this.loading = false;
      });
    }
  }

  Cancel() {
    this.catleForm.reset();
    this.catleForm.controls.name.setErrors(null);
    this.catleForm.controls.location.setErrors(null);
    this.catleForm.controls.country.setErrors(null);
    this.catleForm.controls.dateBuilt.setErrors(null);
    this.catleForm.controls.fileName.setErrors(null);
    this.catleForm.controls.fileSource.setErrors(null);
    this.catleForm.controls.file.setErrors(null);
  }

  build(){
    this.catleForm = this.formBuilder.group({
      name: [null, [Validators.required]],
      file: [null, [Validators.required]],
      fileSource: [null, [Validators.required]],
      fileName: [null, [Validators.required]],
      location: [null, Validators.required],
      country: [null, [Validators.required]],
      dateBuilt: [null, [Validators.required, Validators.pattern(this.numericPattern)]]
    });
  }

  onFileChange(event) {

    if (event.target.files.length > 0) {

      const selected = event.target.files[0];

      this.catleForm.patchValue({

        fileSource: selected,
        fileName: selected.name

      });

    }
  }

}
