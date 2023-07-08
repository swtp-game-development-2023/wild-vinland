import http.server
import gzip
import mimetypes
import os

class MyHTTPRequestHandler(http.server.SimpleHTTPRequestHandler):
    def send_head(self):
        path = self.translate_path(self.path)
        if os.path.isdir(path):
            if not self.path.endswith("/"):
                self.send_response(301)
                self.send_header("Location", self.path + "/")
                self.end_headers()
                return None
            else:
                for index in ["index.html"]:
                    index_path = os.path.join(path, index)
                    if os.path.exists(index_path):
                        path = index_path
                        break
                else:
                    return self.list_directory(path)

        mimetype, encoding = mimetypes.guess_type(path)
        if encoding == "gzip":
            try:
                with open(path, "rb") as f_in:
                    f_in.seek(0, 2)
                    file_size = f_in.tell()
                    f_in.seek(0)
                    data = f_in.read()
                self.send_response(200)
                self.send_header("Content-type", mimetype)
                self.send_header("Content-Encoding", "gzip")
                self.send_header("Content-Length", str(file_size))
                self.end_headers()
                self.wfile.write(data)
            except IOError:
                self.send_error(404, "File not found")
        else:
            return super().send_head()

if __name__ == "__main__":
    http.server.test(HandlerClass=MyHTTPRequestHandler)
