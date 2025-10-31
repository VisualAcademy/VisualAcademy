
// Smooth scroll for subnav
document.querySelectorAll('[data-scrollto]').forEach(a => {
  a.addEventListener('click', e => {
    e.preventDefault();
    const id = a.getAttribute('data-scrollto');
    const el = document.getElementById(id);
    if (el) el.scrollIntoView({behavior:'smooth', block:'start'});
    document.querySelectorAll('.tab').forEach(t=>t.classList.remove('active'));
    a.classList.add('active');
  });
});

// FAQ accordion
document.querySelectorAll('.acc-btn').forEach(btn => {
  btn.addEventListener('click', () => {
    const item = btn.closest('.acc-item');
    const panel = item.querySelector('.acc-panel');
    const open = panel.style.display === 'block';
    // close others
    document.querySelectorAll('.acc-panel').forEach(p => p.style.display = 'none');
    document.querySelectorAll('.chev').forEach(c => c.style.transform = 'rotate(0deg)');
    if (!open){
      panel.style.display = 'block';
      item.querySelector('.chev').style.transform = 'rotate(90deg)';
    }
  });
});

// Tiny helper: copy roadmap as checklist
const copyBtn = document.getElementById('copy-roadmap');
if (copyBtn){
  copyBtn.addEventListener('click', async () => {
    const lines = [...document.querySelectorAll('.step')].map((s,i)=> {
      const title = s.querySelector('h3')?.textContent?.trim() || `Step ${i+1}`;
      return `${i+1}. ${title}`;
    });
    try {
      await navigator.clipboard.writeText(lines.join('\n'));
      copyBtn.textContent = '복사됨!';
      setTimeout(()=>copyBtn.textContent='로드맵 복사', 1200);
    } catch { alert('복사 실패'); }
  });
}
