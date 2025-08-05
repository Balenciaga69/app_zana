import os
import sys


def convert_to_utf8_bom(file_path):
    """
    將檔案編碼從 UTF-8 轉換為 UTF-8-BOM。
    """
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        with open(file_path, 'w', encoding='utf-8-sig') as f:
            f.write(content)
        print(f"✅ 已成功轉換: {file_path}")
        return True
    except UnicodeDecodeError:
        print(f"❌ 無法讀取檔案 (非UTF-8編碼?): {file_path}")
        return False
    except Exception as e:
        print(f"❌ 處理檔案時發生錯誤: {file_path}, 錯誤訊息: {e}")
        return False


def should_skip_file(file_path):
    """
    檢查檔案是否已經是 UTF-8 BOM。
    """
    try:
        with open(file_path, 'rb') as f:
            return f.read(3) == b'\xef\xbb\xbf'
    except Exception as e:
        print(f"❌ 檢查檔案時發生錯誤: {file_path}, 錯誤訊息: {e}")
        return True  # 發生錯誤時選擇跳過


def find_target_files(root_dir, extensions):
    """
    遞迴尋找所有指定副檔名的檔案。
    """
    for dirpath, _, filenames in os.walk(root_dir):
        for filename in filenames:
            if filename.lower().endswith(extensions):
                yield os.path.join(dirpath, filename)


def main(root_directory, extensions=('.cs', '.json', '.yaml', '.yml','.ts','.tsx')):
    total_converted = 0
    total_skipped = 0

    print(f"📂 開始掃描目錄: {root_directory}")
    print("-" * 30)

    for file_path in find_target_files(root_directory, extensions):
        if should_skip_file(file_path):
            print(f"➡️ 跳過 (已是UTF-8-BOM): {file_path}")
            total_skipped += 1
            continue
        if convert_to_utf8_bom(file_path):
            total_converted += 1

    print("-" * 30)
    print(f"✅ 任務完成！共轉換 {total_converted} 個檔案，跳過 {total_skipped} 個檔案。")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("💡 使用方法: python your_script_name.py <目標目錄路徑>")
        root_directory = os.getcwd()
        print(f"⚠️ 未指定目標目錄，將使用當前目錄: {root_directory}")
    else:
        root_directory = sys.argv[1]

    if not os.path.isdir(root_directory):
        print(f"❌ 錯誤: 指定的路徑不是一個有效的目錄: {root_directory}")
    else:
        main(root_directory)
