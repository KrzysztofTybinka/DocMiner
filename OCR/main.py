from flask import Flask, request, jsonify
from flask_cors import CORS
from vision.text_extract import extract_text_from_pdf
from werkzeug.utils import secure_filename

app = Flask(__name__)
CORS(app)

@app.route("/ocr", methods=['POST'])
def home():
    if 'file' not in request.files:
        return jsonify({"error": "No file provided"}), 400
    
    file = request.files['file']
    extracted_text = extract_text_from_pdf(file)
    return jsonify({"text": extracted_text}), 200