import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ApiService } from 'app/api.service';
import { NotificationService } from 'app/notification.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.css']
})
export class UploadFileComponent implements OnInit {

  selectedFiles?: FileList;
  currentFile?: File;
  progress = 0;
  message = '';

  fileInfos?: Observable<any>;

  @ViewChild('chooseFile') chooseFile: ElementRef;

  constructor(private apiService: ApiService, private http: HttpClient, private notifyService: NotificationService) { }

  ngOnInit(): void {
  }

  selectFile(event: any): void {
    this.selectedFiles = event.target.files;
  }

  upload(): void {
    this.progress = 0;
  
    if (this.selectedFiles) {
      const file: File | null = this.selectedFiles.item(0);
  
      if (file) {
        this.currentFile = file;
        const formData = new FormData();
        formData.append('file', this.currentFile, this.currentFile.name);
  
        this.apiService.uploadFile(formData).subscribe(
          (event: any) => {
            if (event.type === HttpEventType.UploadProgress) {
              this.progress = Math.round(100 * event.loaded / event.total);
              this.notifyService.showSuccess("Employees updated successfully!", "");
             } 
             //else if (event instanceof HttpResponse) {
            //   this.message = event.body.message;
            //   console.log(this.message);
            // }
          },
          (err: any) => {
            console.log(err);
            this.progress = 0;
  
            if (err.error && err.error.message) {
              this.message = err.error.message;
            } else {
              this.message = 'Could not upload the file!';
              this.notifyService.showError("Could not update employees!", "");
            }
  
            this.currentFile = undefined;
          });
      }
  
      this.selectedFiles = undefined;
    }
  }

  clearUploadFile(): void {
    this.chooseFile.nativeElement.value = '';
  }

}
